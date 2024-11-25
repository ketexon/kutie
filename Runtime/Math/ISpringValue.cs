namespace Kutie {
	public interface ISpringValue<TValue, TVelocity> {
		public TValue CurrentValue { get; }
		public TValue TargetValue { get; set; }
		public TVelocity Velocity { get; set; }

		public float K1 { get; set; }
		public float K2 { get; set; }
		public float K3 { get; set; }

		public float Omega { get; set; }
		public float Zeta { get; set; }
		public float R { get; set; }

		public void SetConstants(float k1, float k2, float k3);
		public void SetParameters(float omega, float zeta, float r);
		public void SetParameters(SpringParameters parameters);

		public void LockToTarget();
	}
}