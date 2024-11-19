using UnityEngine;

namespace Kutie {
    /// <summary>
    /// Utility class to add a static <see cref="Instance"/> member referring to the instance of this component.
    /// If a new component is instantiated, destroy the new one if <see cref="DestroyNewInstance"/> is true, otherwise destroy
    /// the old instance.
    /// </summary>
    /// <typeparam name="T">Child class</typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
        where T : SingletonMonoBehaviour<T>
    {
        public static UnityEngine.Events.UnityEvent<T> NewInstanceEvent = new();
        public static T Instance { get; private set; }

        /// <summary>
        /// If true, then Destroy the current instance of the component if 
        /// <see cref="Instance"/> is already set. Otherwise, Destroy the old instance.
        /// </summary>
        virtual protected bool DestroyNewInstance => true;

        virtual protected void Awake()
        {
            if (Instance)
            {
                if (DestroyNewInstance)
                {
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    Destroy(Instance.gameObject);
                }
            }
            Instance = this as T;
            NewInstanceEvent.Invoke(Instance);
        }
    }
}
