using System;
using UnityEngine;

namespace Kutie.Singleton {
    public class SingletonScriptableObject<T> : ScriptableObject
        where T : SingletonScriptableObject<T>
    {
        static WeakReference<T> _weakInstance = new(null);

        public static T Instance {
            get {
                if(_weakInstance.TryGetTarget(out T instance)){
                    return instance;
                }
                return null;
            }
        }

        virtual protected void OnEnable()
        {
            if(Instance != null && Instance != this)
            {
                Debug.LogWarning($"Multiple instances of SingletonObject {typeof(T).Name}");
            }
            _weakInstance.SetTarget(this as T);
        }
    }
}
