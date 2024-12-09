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

		public bool Angle = false;
		public float ProportionalGain = 1;
		public float IntegralGain = 0;
		public float DerivativeGain = 0;
		public float MaxIntegral = 1;

		public float MinOutput = -1;
		public float MaxOutput = 1;
	}

	public interface IPIDController<T> {
		public T TargetValue { get; set; }
		public T CurrentValue { get; set; }
		public T Output { get; }
		public T Update(float dt);
	}

	[System.Serializable]
	public class PIDFloatController : IPIDController<float> {
		[SerializeField]
		PIDParameters _parameters = new();
		public PIDParameters Parameters {
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
			float I = Parameters. IntegralGain * integral;

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
		PIDParameters _parameters = new();
		public PIDParameters Parameters {
			get => _parameters;
			set {
				_parameters = value;
				xController.Parameters = value;
				yController.Parameters = value;
				zController.Parameters = value;
			}
		}

		[SerializeField, HideInInspector]
		PIDFloatController xController = new();

		[SerializeField, HideInInspector]
		PIDFloatController yController = new();

		[SerializeField, HideInInspector]
		PIDFloatController zController = new();

		public Vector3 TargetValue {
			get => new(xController.CurrentValue, yController.CurrentValue, zController.CurrentValue);
			set {
				xController.TargetValue = value.x;
				yController.TargetValue = value.y;
				zController.TargetValue = value.z;
			}
		}

		public Vector3 CurrentValue {
			get => new(xController.CurrentValue, yController.CurrentValue, zController.CurrentValue);
			set {
				xController.CurrentValue = value.x;
				yController.CurrentValue = value.y;
				zController.CurrentValue = value.z;
			}
		}

		public Vector3 Output {
			get => new(xController.Output, yController.Output, zController.Output);
		}

		public Vector3 Update(float dt) {
			xController.Update(dt);
			yController.Update(dt);
			zController.Update(dt);

			return Output;
		}
	}
}