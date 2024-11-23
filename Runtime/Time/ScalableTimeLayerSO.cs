using UnityEngine;
using UnityEngine.Events;

namespace Kutie.Time {
	[CreateAssetMenu(menuName = "Kutie/Time/Scalable Time Layer", fileName = "ScalableTimeLayer")]
	public class ScalableTimeLayerSO : TimeLayerSO {
		protected override void OnEnable(){
			Value = new ScalableTimeLayer();
		}

		public UnityEvent<float> ScaleChangedEvent => (Value as ScalableTimeLayer).ScaleChangedEvent;

	}
}