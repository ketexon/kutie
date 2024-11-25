using UnityEngine;

namespace Kutie {
	public class SpringFloatValue : MonoBehaviour, ISpringValue<float, float> {
		[SerializeField] float _defaultTarget = 0;
		[SerializeField] SpringParameters _defaultParameters;

		public float CurrentValue {
			get => Spring.CurrentValue;
			set {
				Spring.TargetValue = value;
				Spring.LockToTarget();
			}
		}

		public float TargetValue {
			get => Spring.TargetValue;
			set {
				Spring.TargetValue = value;
			}
		}

        public float Velocity { get => Spring.Velocity; set => Spring.Velocity = value; }

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

		public float K1 { get => Spring.K1; set => Spring.K1 = value; }
		public float K2 { get => Spring.K2; set => Spring.K2 = value; }
		public float K3 { get => Spring.K3; set => Spring.K3 = value; }

		public float Omega { get => Spring.Omega; set => Spring.Omega = value; }
		public float Zeta { get => Spring.Zeta; set => Spring.Zeta = value; }
		public float R { get => Spring.R; set => Spring.R = value; }

		public void LockToTarget() => Spring.LockToTarget();


        [System.NonSerialized] public SpringFloat Spring = null;

		void Awake() {
			Spring = new(
				_defaultTarget,
				_defaultParameters
			);
		}

		void Update() {
			Spring.Update(UnityEngine.Time.deltaTime);
		}

        public void SetConstants(float k1, float k2, float k3)
        {
            throw new System.NotImplementedException();
        }

        public void SetParameters(float omega, float zeta, float r)
        {
            throw new System.NotImplementedException();
        }

        public void SetParameters(SpringParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}