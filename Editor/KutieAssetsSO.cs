using Kutie.Singleton;
using UnityEngine;

namespace Kutie.Editor {
	[CreateAssetMenu]
	public class KutieAssetsSO : SingletonScriptableObject<KutieAssetsSO> {
		public Sprite Warning;
	}
}