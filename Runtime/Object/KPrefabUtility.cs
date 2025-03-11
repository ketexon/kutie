using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kutie {
	public static partial class KPrefabUtility {
		/// <summary>
		/// Functions as both PrefabUtility.InstantiatePrefab and Object.Instantiate.
		/// In the editor, it uses PrefabUtility.InstantiatePrefab to instantiate the prefab.
		/// In play mode, it uses Object.Instantiate to instantiate the prefab.
		/// This is because PrefabUtility.InstantiatePrefab does not work in play mode.
		/// </summary>
		/// <param name="monoBehaviour">The MonoBehaviour instance calling this method.</param>
		/// <param name="prefab">The prefab to instantiate.</param>
		/// <param name="position">The position to place the instantiated object. If null, the prefab's original position is used.</param>
		/// <param name="rotation">The rotation to apply to the instantiated object. If null, the prefab's original rotation is used.</param>
		/// <param name="parent">The parent transform to set for the instantiated object. If null, no parent is set.</param>
		/// <returns>The instantiated GameObject.</returns>
		public static GameObject InstantiatePrefab(
			GameObject prefab,
			Vector3? position = null,
			Quaternion? rotation = null,
			Transform parent = null
		){
			GameObject SpawnNormally(){
				if(parent){
					return Object.Instantiate(
						prefab,
						position ?? prefab.transform.position,
						rotation ?? prefab.transform.rotation,
						parent
					);
				}
				else {
					return Object.Instantiate(
						prefab,
						position ?? prefab.transform.position,
						rotation ?? prefab.transform.rotation
					);
				}
			}
			#if UNITY_EDITOR
			if(EditorApplication.isPlaying){
				return SpawnNormally();
			}
			var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
			if(parent){
				go.transform.SetParent(parent);
			}
			go.transform.SetPositionAndRotation(
				position ?? prefab.transform.position,
				rotation ?? prefab.transform.rotation
			);
			return go;
			#else
			return SpawnNormally();
			#endif
		}
	}
}