using System.Collections.Generic;

namespace Kutie.Collections {
	public class KTree<T> {
		[System.Serializable]
		public class Node {
			public T Value;
			public List<Node> Children = new();
			public Node Parent = null;
		}

		public Node Root { get; private set; } = new();
	}
}