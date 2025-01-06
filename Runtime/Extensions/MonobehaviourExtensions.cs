using System.Collections;
using UnityEngine;

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
                Object.Destroy(child.gameObject);
            }
        }
    }
}
