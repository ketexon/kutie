using UnityEngine;

namespace Kutie {
	public class PIDFollow : MonoBehaviour {
		[SerializeField] PIDVector3Controller controller;
		[SerializeField] Transform target;

		void Update() {
			controller.TargetValue = target.position;
			transform.position = controller.CurrentValue;

			controller.Update(UnityEngine.Time.deltaTime);
		}
	}
}