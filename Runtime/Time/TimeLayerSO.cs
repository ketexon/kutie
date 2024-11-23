using UnityEngine;
using UnityEngine.Events;

namespace Kutie.Time {
	[CreateAssetMenu(menuName = "Kutie/Time/Time Layer", fileName = "TimeLayer")]
	public class TimeLayerSO : ScriptableObject {
		[System.NonSerialized] public TimeLayer Value = null;

		protected virtual void OnEnable(){
			Value = new TimeLayer();
		}

		public UnityEvent<bool> PausedChangedEvent => Value.PausedChangedEvent;
		public UnityEvent TimeZeroedEvent => Value.TimeZeroedEvent;
		public UnityEvent FixedTimeZeroedEvent => Value.FixedTimeZeroedEvent;

		public bool Paused {
			get => Value.Paused;
			set => Value.Paused = value;
		}

		public float Time => Value.Time;
		public float FixedTime => Value.FixedTime;
		public float DeltaTime => Value.DeltaTime;
		public float FixedDeltaTime => Value.FixedDeltaTime;
	}
}