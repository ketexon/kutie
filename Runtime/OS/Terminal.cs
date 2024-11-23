using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kutie.OS
{
    public static class Terminal
    {
        /// <summary>
        /// Get the sensible terminal for the current system, based on https://github.com/i3/i3/blob/next/i3-sensible-terminal.
        /// Note that this is slightly opinionated, and I put terminals I prefer at the top of the list.
        /// </summary>
        /// <remarks>
        /// NOTE: THIS IS AN EXTREME SECURITY VULERNABILITY IF USED AT RUNTIME.
        /// Settings the $TERM environment variable would let an attacker
        /// choose an arbitrary terminal.
        /// </remarks>
        /// <returns>full path to the terminal, else null</returns>
        public static string GetSensibleTerminal(){
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
            var termVar = System.Environment.GetEnvironmentVariable("TERM");
            var terminalVar = System.Environment.GetEnvironmentVariable("TERMINAL");
            string[] terminals = {
                termVar,
                terminalVar,
                "kitty",
                "alacritty",
                "urxvt",
                "x-terminal-emulator",
                "mate-terminal",
                "gnome-terminal",
                "terminator",
                "xfce4-terminal",
                "termit",
                "Eterm",
                "aterm",
                "uxterm",
                "xterm",
                "roxterm",
                "termite",
                "lxterminal",
                "terminology",
                "st",
                "qterminal",
                "lilyterm",
                "tilix",
                "terminix",
                "konsole",
                "guake",
                "tilda",
                "hyper",
                "wezterm",
                "rxvt",
            };

            foreach (string terminal in terminals){
                var which = Executable.Which(terminal);
                if (which != null){
                    return which;
                }
            }

            return null;
#else
            throw new System.NotImplementedException("Platform not supported");
#endif
        }
    }
}
