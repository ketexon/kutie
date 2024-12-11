using System.Collections.Generic;

namespace Kutie.Extensions {
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

		public static IEnumerable<(int, T)> ZipIndex<T>(this IEnumerable<T> list) {
			int i = 0;
			foreach (var item in list) {
				yield return (i, item);
				i++;
			}
		}

		public static IEnumerable<U> Map<T, U>(this IEnumerable<T> list, System.Func<T, U> func) {
			foreach (var item in list) {
				yield return func(item);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> list, System.Action<T> action) {
			foreach (var item in list) {
				action(item);
			}
		}

		public static List<T> CollectList<T>(this IEnumerable<T> list) {
			return new List<T>(list);
		}
	}
}