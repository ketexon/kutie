using System.Collections.Generic;

namespace Kutie {
	public static class CollectionExtensions {
		public static T Sample<T>(this IList<T> list) {
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		public static List<T> Sample<T>(this List<T> list, int n) {
			return list.Shuffle().GetRange(0, n);
		}

		public static List<T> Shuffle<T>(this List<T> list){
			list = new(list);
			return list.ShuffleInPlace();
        }

		public static List<T> ShuffleInPlace<T>(this List<T> list){
			for (int i = 0; i < list.Count; i++) {
				int j = UnityEngine.Random.Range(i, list.Count);
                (list[j], list[i]) = (list[i], list[j]);
            }
			return list;
		}
	}
}