using System.Collections.Generic;
using Kutie.Collections;
using UnityEngine;

namespace Kutie {
	public static partial class Algorithm {
		public static List<Vector2Int> AStar(
			Vector2Int startCell,
			System.Func<Vector2Int, float> heuristic,
			System.Func<Vector2Int, bool> isTarget,
			System.Func<Vector2Int, bool> isWalkable
		){
			// cost from startCell to cell
			var gScore = new Dictionary<Vector2Int, float>() {
				[startCell] = 0
			};
			// estimated cost from startCell to cell
			var fScore = new Dictionary<Vector2Int, float>() {
				[startCell] = heuristic(startCell)
			};

			var pq = new KPriorityQueue<Vector2Int, float>();
			var visited = new HashSet<Vector2Int>();
			var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

			pq.Enqueue(startCell, fScore[startCell]);

			Vector2Int current = default;
			while(pq.Count > 0){
				current = pq.Dequeue();

				if(isTarget(current)){
					break;
				}

				visited.Add(current);

				foreach(var dir in KMath.Directions4){
					var neighbor = current + dir;
					if(!isWalkable(neighbor) || visited.Contains(neighbor)){
						continue;
					}

					var tentativeGScore = gScore[current] + 1;
					var contains = pq.Contains(neighbor);
					if(contains){
						if(tentativeGScore >= gScore[neighbor]){
							continue;
						}
						else {
							pq.Remove(neighbor, out _, out _);
						}
					}

					cameFrom[neighbor] = current;
					gScore[neighbor] = tentativeGScore;
					fScore[neighbor] = gScore[neighbor] + heuristic(neighbor);
					pq.Enqueue(neighbor, fScore[neighbor]);
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

		public static List<Vector2Int> BFS(
			Vector2Int startCell,
			System.Func<Vector2Int, bool> isTarget,
			System.Func<Vector2Int, bool> isWalkable
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