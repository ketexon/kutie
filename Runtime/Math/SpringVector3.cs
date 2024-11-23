using UnityEngine;

namespace Kutie {
	/// <summary>
	/// Adapted from https://www.youtube.com/watch?v=KPoeNZZ6H4s
	/// </summary>
	public class SpringVector3 {
		public Vector3 CurrentValue { get; private set; }
		public Vector3 TargetValue { get; set; }
		public Vector3 Velocity { get; private set; }

		Vector3 previousTargetValue;

		// y + 2 * zeta/omega * y' + 1/omega^2 * y'' = x + r*zera/omega * x'
		// k1 = 2*zeta / omega
		// k2 = 1 / omega^2
		// k3 = r*zeta / omega
		// omega = 1 / sqrt(k2)
		// zeta = k1 * omega / 2
		// r = 2 * k3 / k1
		float _k1, _k2, _k3;
		float _omega, _zeta, _r;

		public float K1 {
			get => _k1;
			set {
				if(_k1 == value) return;
				SetConstants(value, _k2, _k3);
			}
		}

		public float K2 {
			get => _k2;
			set {
				if(_k2 == value) return;
				SetConstants(_k1, value, _k3);
			}
		}

		public float K3 {
			get => _k3;
			set {
				if(_k3 == value) return;
				SetConstants(_k1, _k2, value);
			}
		}

		public float Omega {
			get => _omega;
			set {
				if(_omega == value) return;
				SetParameters(value, _zeta, _r);
			}
		}

		public float Zeta {
			get => _zeta;
			set {
				if(_zeta == value) return;
				SetParameters(_omega, value, _r);
			}
		}

		public float R {
			get => _r;
			set {
				if(_r == value) return;
				SetParameters(_omega, _zeta, value);
			}
		}

		// for pole matching
		float d;

		public SpringVector3(Vector3 initialValue, float omega, float zeta, float r) {
			CurrentValue = initialValue;
			TargetValue = initialValue;
			previousTargetValue = initialValue;
			Velocity = Vector3.zero;

			SetParameters(omega, zeta, r);
		}

		public SpringVector3 Clone(){
			return new SpringVector3(CurrentValue, Omega, Zeta, R);
		}

		public void SetConstants(float k1, float k2, float k3) {
			_k1 = k1;
			_k2 = k2;
			_k3 = k3;

			_omega = 1 / Mathf.Sqrt(k2);
			_zeta = k1 * _omega / 2;
			_r = 2 * k3 / k1;

			UpdatePoleMatchingConstant();
		}

		public void SetParameters(float omega, float zeta, float r) {
			_omega = omega;
			_zeta = zeta;
			_r = r;

			_k1 = 2 * zeta / omega;
			_k2 = 1 / (omega * omega);
			_k3 = r * zeta / omega;

			UpdatePoleMatchingConstant();
		}

		void UpdatePoleMatchingConstant(){
			d = Omega * Mathf.Sqrt(Mathf.Abs(1 - Zeta * Zeta));
		}

		public void Update(float deltaTime) {
			Vector3 targetValueDot = (TargetValue - previousTargetValue)/deltaTime;
			previousTargetValue = TargetValue;

			float k1Stable, k2Stable;
			if(Omega * deltaTime < Zeta){
				k1Stable = K1;
				k2Stable = Mathf.Max(K2, deltaTime * deltaTime/2 + deltaTime * K2/2, deltaTime * K1);
			}
			else {
				float t1 = Mathf.Exp(-Zeta * Omega * deltaTime);
				float alpha = 2 * t1 * (Zeta <= 1.0 ? Mathf.Cos(deltaTime * d) : Cosh(deltaTime * d));
				float beta = t1 * t1;
				float t2 = deltaTime / (1 + beta - alpha);
				k1Stable = (1 - beta) * t2;
				k2Stable = deltaTime * t2;
			}
			CurrentValue += deltaTime * Velocity;
			Velocity += deltaTime * (TargetValue + K3*targetValueDot - CurrentValue - k1Stable * Velocity)/k2Stable;
		}

		float Cosh(float x) => (Mathf.Exp(x) + Mathf.Exp(-x))/2;
	}
}