namespace Kutie {
	[System.Serializable]
	public class IntRange {
		public int Min;
		public int MaxExclusive;

		public int MaxInclusive => MaxExclusive - 1;
		public int Max => MaxInclusive;

		public IntRange(int min, int maxExclusive) {
			Min = min;
			MaxExclusive = maxExclusive;
		}

		public int Random() {
			return UnityEngine.Random.Range(Min, Max);
		}
	}
}