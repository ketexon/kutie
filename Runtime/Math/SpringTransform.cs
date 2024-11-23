using UnityEngine;

namespace Kutie {

	public class SpringTransform : MonoBehaviour {
		[SerializeField] Transform target;
		[SerializeField] float _frequency = 1.0f;

		[SerializeField, Min(0)] float _dampingCoefficient = 0.5f;
		[SerializeField] float _responsiveness = 0;

		public float Frequency {
			get => Spring.Omega / (2 * Mathf.PI);
			set {
				if(Frequency == value) return;
				Spring.Omega = 2 * Mathf.PI * value;
			}
		}

		public float DampingCoefficient {
			get => Spring.Zeta;
			set {
				if(DampingCoefficient == value) return;
				Spring.Zeta = value;
			}
		}

		public float Responsiveness {
			get => Spring.R;
			set {
				if(Responsiveness == value) return;
				Spring.R = value;
			}
		}

		[System.NonSerialized] public SpringVector3 Spring = null;

		void Awake() {
			Spring = new(
				target.position,
				2 * Mathf.PI * _frequency,
				_dampingCoefficient,
				_responsiveness
			);
		}

		void Update() {
			Spring.TargetValue = target.position;
			Spring.Update(Time.deltaTime);
			transform.position = Spring.CurrentValue;
		}
	}
}