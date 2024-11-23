using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kutie.Time
{
    [System.Serializable]
    public class TimeLayer
    {
        public UnityEvent<bool> PausedChangedEvent;
        public UnityEvent TimeZeroedEvent;
        public UnityEvent FixedTimeZeroedEvent;

        public virtual bool Paused {
            get => _paused;
            set {
                if (_paused == value) return;
                _paused = value;
                if(_paused){
                    _lastPauseUnityTime = UnityEngine.Time.time;
                    _lastPauseUnityFixedTime = UnityEngine.Time.fixedTime;
                }

                else {
                    _totalPauseDuration += UnityEngine.Time.time - _lastPauseUnityTime;
                    _totalPauseFixedDuration += UnityEngine.Time.fixedTime - _lastPauseUnityFixedTime;
                }
                PausedChangedEvent.Invoke(_paused);
            }
        }

        public virtual float Time
            => (
                _paused
                    ? _lastPauseUnityTime
                    : UnityEngine.Time.time
            ) - _totalPauseDuration;

        public virtual float FixedTime
            => (
                _paused
                    ? _lastPauseUnityFixedTime
                    : UnityEngine.Time.fixedTime
            ) - _totalPauseFixedDuration;

        public virtual float DeltaTime
            => _paused
            ? 0f
            : UnityEngine.Time.deltaTime;

        public virtual float FixedDeltaTime
            => _paused
            ? 0f
            : UnityEngine.Time.fixedDeltaTime;

        [SerializeField]
        bool _paused = false;

        // The Time.time of the last pause
        [SerializeField, HideInInspector]
        float _lastPauseUnityTime = 0f;

        // The Time.fixedTime of the last pause
        [SerializeField, HideInInspector]
        float _lastPauseUnityFixedTime = 0f;

        // The total amount of time paused.
        // Calculated via the difference between Time.time and _lastPauseUnityTime every
        // time resumed.
        [SerializeField, HideInInspector]
        float _totalPauseDuration = 0f;

        //Total amount of fixed time paused (see _totalPauseDuration)
        [SerializeField, HideInInspector]
        float _totalPauseFixedDuration = 0f;

        public void Zero()
        {
            ZeroTime();
            ZeroFixedTime();
        }

        public void ZeroTime()
        {
            _totalPauseDuration = UnityEngine.Time.time;
            TimeZeroedEvent?.Invoke();
        }

        public void ZeroFixedTime()
        {
            _totalPauseFixedDuration = UnityEngine.Time.fixedTime;
            TimeZeroedEvent?.Invoke();
        }
    }
}
