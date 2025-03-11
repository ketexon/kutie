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
            foreach (Transform child in behaviour.transform)
            {
                behaviour.DestroySmart(child.gameObject);
            }
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
