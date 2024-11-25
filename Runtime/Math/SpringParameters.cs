using UnityEngine;

namespace Kutie {
	[System.Serializable]
	public class SpringParameters {
		[Min(0.1f)]
		public float Frequency = 1;
		[Min(0)]
		public float DampingCoefficient = 1;
		public float Responsiveness = 0;

		public float Omega {
			get => 2 * Mathf.PI * Frequency;
			set {
				Frequency = value / (2 * Mathf.PI);
			}
		}

		public float K1 => 2 * DampingCoefficient / Omega;
		public float K2 => 1 / (Omega * Omega);
		public float K3 => Responsiveness * DampingCoefficient / Omega;
	}
}