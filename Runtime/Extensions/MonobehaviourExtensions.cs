using System.Collections;
using UnityEngine;

namespace Kutie.Extensions
{
    public static class MonobehaviourExtensions
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
    }
}
