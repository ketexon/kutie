#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Kutie {
    public class CommandPromptEditorUtil
    {
        [MenuItem("Assets/Open in terminal")]
        public static void OpenInTerminal()
        {
            Object activeObject = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(activeObject);
            // Application.dataPath include Assets, but path does too
            var rootPath = Directory.GetParent(Application.dataPath).FullName;
            var assetPath = Path.Combine(rootPath, path);
            string directoryPath = Directory.Exists(assetPath)
                ? assetPath
                : Directory.GetParent(assetPath).FullName;

            directoryPath = Path.GetFullPath(directoryPath);

#if UNITY_EDITOR_WIN
            // try to open Windows Terminal
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "wt.exe",
                    Arguments = @$"-d {'"'}{directoryPath}{'"'}",
                });
                return;
            }
            catch(System.ComponentModel.Win32Exception)
            {}

            // try to open Powershell 7
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "pwsh.exe",
                    WorkingDirectory = directoryPath,
                });
                return;
            }
            catch (System.ComponentModel.Win32Exception)
            { }

            // try to open Powershell 5
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    WorkingDirectory = directoryPath,
                });
                return;
            }
            catch (System.ComponentModel.Win32Exception)
            {}

            // try to open Command Prompt (if this doesn't work, god is dead)
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = directoryPath,
                });
                return;
            }
            catch (System.ComponentModel.Win32Exception)
            {}

            throw new System.NotSupportedException("Could not open terminal");
#elif UNITY_EDITOR_LINUX
            var terminal = OS.Terminal.GetSensibleTerminal();
            if(terminal != null){
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = terminal,
                    WorkingDirectory = directoryPath,
                });
                return;
            }

            throw new System.NotSupportedException("Could not open terminal");
#else
            Debug.LogError("Cannot open console on your platform: not supported");
#endif
        }
    }
}
#endif
