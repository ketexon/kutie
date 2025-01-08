using UnityEngine;

namespace Kutie {
	public class PIDAccelerationFollow : MonoBehaviour {
		[SerializeField] PIDVector3Controller controller;
		[SerializeField] Transform target;
		[SerializeField] new Rigidbody rigidbody;

		void Update() {
			controller.TargetValue = target.position;
			controller.CurrentValue = transform.position;

			var output = controller.Update(UnityEngine.Time.deltaTime);
			rigidbody.AddForce(output, ForceMode.Acceleration);
		}
	}
}