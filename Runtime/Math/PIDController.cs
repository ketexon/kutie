using UnityEngine;

namespace Kutie {
	[System.Serializable]
	public enum PIDControllerDerivativeMode {
		Value,
		Error
	}

	[System.Serializable]
	public class PIDParameters {
		public PIDControllerDerivativeMode DerivativeMode = PIDControllerDerivativeMode.Value;

		public float ProportionalGain = 1;
		public float IntegralGain = 0;
		public float DerivativeGain = 0;
		public float MaxIntegral = 1;

		public float MaxOutput = 1;
	}

	[System.Serializable]
	public class PIDFloatParameters : PIDParameters {
		public bool Angle = false;
		public float MinOutput = -1;
	}

	[System.Serializable]
	public class PIDVector3Parameters : PIDParameters {}

	public interface IPIDController<T> {
		public T TargetValue { get; set; }
		public T CurrentValue { get; set; }
		public T Output { get; }
		public T Update(float dt);
	}

	[System.Serializable]
	public class PIDFloatController : IPIDController<float> {
		[SerializeField]
		PIDFloatParameters _parameters = new();
		public PIDFloatParameters Parameters {
			get => _parameters;
			set => _parameters = value;
		}

		public float TargetValue { get; set; } = 0;
		public float CurrentValue { get; set; } = 0;

		public float Output { get; private set; } = 0;

		bool initialized = false;
		float lastValue = 0;
		float lastError = 0;

		float integral = 0;

		public float Update(float dt) {
			float error = TargetValue - CurrentValue;
			if(Parameters.Angle) {
				error = KMath.NormalizeAngle180(error);
			}

			float P = Parameters.ProportionalGain * error;

			integral += error * dt;
			integral = Mathf.Clamp(integral, -Parameters.MaxIntegral, Parameters.MaxIntegral);
			float I = Parameters.IntegralGain * integral;

			float D = 0;
			if(initialized){
				float delta;
				if(Parameters.DerivativeMode == PIDControllerDerivativeMode.Value) {
					delta = -(CurrentValue - lastValue);
					if(Parameters.Angle) {
						delta = KMath.NormalizeAngle180(delta);
					}
				} else {
					delta = error - lastError;
					if(Parameters.Angle) {
						delta = KMath.NormalizeAngle180(delta);
					}
				}
				D = Parameters.DerivativeGain * delta / dt;
			}
			else {
				initialized = true;
			}

			Output = Mathf.Clamp(P + I + D, Parameters.MinOutput, Parameters.MaxOutput);

			lastError = error;
			lastValue = CurrentValue;

			return Output;
		}
	}

	[System.Serializable]
	public class PIDVector3Controller : IPIDController<Vector3> {
		[SerializeField]
		PIDVector3Parameters _parameters = new();
		public PIDVector3Parameters Parameters {
			get => _parameters;
			set => _parameters = value;
		}

		public Vector3 TargetValue { get; set; } = Vector3.zero;
		public Vector3 CurrentValue { get; set; } = Vector3.zero;

		public Vector3 Output { get; private set; } = Vector3.zero;

		bool initialized = false;
		Vector3 lastValue = Vector3.zero;
		Vector3 lastError = Vector3.zero;

		Vector3 integral = Vector3.zero;

		public Vector3 Update(float dt) {
			Vector3 error = TargetValue - CurrentValue;

			Vector3 P = Parameters.ProportionalGain * error;

			integral += error * dt;
			integral = Vector3.ClampMagnitude(
				integral,
				Parameters.MaxIntegral
			);
			Vector3 I = Parameters.IntegralGain * integral;

			Vector3 D = Vector3.zero;
			if(initialized){
				Vector3 delta;
				if(Parameters.DerivativeMode == PIDControllerDerivativeMode.Value) {
					delta = -(CurrentValue - lastValue);
				} else {
					delta = error - lastError;
				}
				D = Parameters.DerivativeGain * delta / dt;
			}
			else {
				initialized = true;
			}

			Output = Vector3.ClampMagnitude(P + I + D, Parameters.MaxOutput);

			lastError = error;
			lastValue = CurrentValue;

			return Output;
		}
	}
}