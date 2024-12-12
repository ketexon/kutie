using System.Collections.Generic;
using Kutie.Collections;
using UnityEngine;

namespace Kutie {
	public static partial class Algorithm {
		public static List<Vector2Int> BFS(
			Vector2Int startCell,
			System.Predicate<Vector2Int> isTarget,
			System.Predicate<Vector2Int> isWalkable
		) {
			var visited = new HashSet<Vector2Int>();
			var queue = new Queue<Vector2Int>();
			var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

			queue.Enqueue(startCell);
			visited.Add(startCell);

			Vector2Int current = default;
			while(queue.Count > 0){
				current = queue.Dequeue();

				if(isTarget(current)){
					break;
				}

				foreach(var dir in KMath.Directions4){
					var neighbor = current + dir;
					if(!isWalkable(neighbor) || visited.Contains(neighbor)){
						continue;
					}

					visited.Add(neighbor);
					cameFrom[neighbor] = current;
					queue.Enqueue(neighbor);
				}
			}
			if(isTarget(current)){
				var path = new List<Vector2Int>();
				while(cameFrom.ContainsKey(current)){
					path.Add(current);
					current = cameFrom[current];
				}
				path.Add(current);
				path.Reverse();
				return path;
			}
			return null;
		}
	}
}