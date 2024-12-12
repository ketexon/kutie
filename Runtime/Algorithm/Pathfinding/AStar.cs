using System.Collections.Generic;
using Kutie.Collections;
using UnityEngine;

namespace Kutie {
	public static partial class Algorithm {
		public static List<Vector2Int> AStar(
			Vector2Int startCell,
			System.Func<Vector2Int, float> heuristic,
			System.Predicate<Vector2Int> isTarget,
			System.Predicate<Vector2Int> isWalkable
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
	}
}