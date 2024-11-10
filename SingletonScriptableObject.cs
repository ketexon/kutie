using System;
using UnityEngine;

namespace Kutie {
    public class SingletonScriptableObject<T> : ScriptableObject
        where T : SingletonScriptableObject<T>
    {
        public static T Instance { get; private set; }

        virtual protected void OnEnable()
        {
            if(Instance != null && Instance != this)
            {
                Debug.LogWarning($"Multiple instances of SingletonObject {typeof(T).Name}");
            }
            Instance = this as T;
        }

        virtual protected void OnDisable()
        {
            Instance = null;
        }
    }
}
