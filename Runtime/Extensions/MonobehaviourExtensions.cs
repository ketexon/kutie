using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kutie.Extensions
{
    public static partial class MonobehaviourExtensions
    {
        public static Coroutine Defer(this MonoBehaviour behaviour, System.Action action, YieldInstruction yieldInstruction = null)
        {
            IEnumerator Impl()
            {
                yield return yieldInstruction;
                action();
            }
            return behaviour.StartCoroutine(Impl());
        }

        public static void ClearChildren(this MonoBehaviour behaviour)
        {
            #if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                foreach (Transform child in behaviour.transform)
                {
                    Object.Destroy(child.gameObject);
                }
            }
            else
            {
                var nChildren = behaviour.transform.childCount;
                for(int i = 0; i < nChildren; ++i){
                    Object.DestroyImmediate(
                        behaviour.transform.GetChild(0).gameObject
                    );
                }
            }
            #else
            foreach (Transform child in behaviour.transform)
            {
                Object.Destroy(child.gameObject);
            }
            #endif
        }

        public static void DestroySmart(this MonoBehaviour behaviour, Object obj)
        {
            #if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                Object.Destroy(obj);
            }
            else
            {
                Object.DestroyImmediate(obj);
            }
            #else
            Object.Destroy(obj);
            #endif
        }
    }
}
