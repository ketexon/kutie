using UnityEngine;
using UnityEngine.Events;

namespace Kutie.Time
{
    public class ScalableTimeLayer : TimeLayer
    {
        public UnityEvent<float> ScaleChangedEvent;

        public float Scale {
            get => _scale;
            set
            {
                if(value < 0)
                {
                    throw new System.ArgumentOutOfRangeException(
                        "ScalableTime.Scale",
                        value,
                        "Scale must be nonnegative"
                    );
                }
                ScaleChanged();
                _scale = value;
                ScaleChangedEvent?.Invoke(_scale);
            }
        }

        // Calculated by storing the base time and scaled time after every
        // scale change, and scaling the delta between the last scale base time
        // and current base time, adding the last saved scaled time
        public override float Time
            => (base.Time - _lastScaleChangedTime) * _scale
            + _lastScaleChangedScaledTime;

        public override float FixedTime
            => (base.Time - _lastScaleChangedFixedTime) * _scale
            + _lastScaleChangedScaledFixedTime;

        public override float DeltaTime => base.DeltaTime * _scale;
        public override float FixedDeltaTime => base.FixedDeltaTime * _scale;


        [SerializeField]
        float _scale = 1f;

        [SerializeField,HideInInspector]
        float _lastScaleChangedTime = 0f;
        [SerializeField,HideInInspector]
        float _lastScaleChangedScaledTime = 0f;

        [SerializeField,HideInInspector]
        float _lastScaleChangedFixedTime = 0f;
        [SerializeField,HideInInspector]
        float _lastScaleChangedScaledFixedTime = 0f;

        void ScaleChanged()
        {
            _lastScaleChangedScaledTime = Time;
            _lastScaleChangedScaledFixedTime = FixedTime;

            _lastScaleChangedTime = base.Time;
            _lastScaleChangedFixedTime = base.FixedTime;
        }
	}
}