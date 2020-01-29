using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
	public static class BfsTask
	{
		public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
		{
			var way = new SinglyLinkedList<Point>(start);
			var visited = new HashSet<Point> { start };
			var queue = new Queue<SinglyLinkedList<Point>>();
			queue.Enqueue(way);

			while (queue.Count != 0)
			{
				var point = queue.Dequeue();
				var moves = point.Value.NearbyMoves(map).Where(p => !visited.Contains(p));
				foreach (var move in moves)
				{
					way = new SinglyLinkedList<Point>(move, point);
					visited.Add(move);
					queue.Enqueue(way);

					if (chests.Contains(move))
						yield return way;
				}
			}
			yield break;
		}

		public static IEnumerable<Point> NearbyMoves(this Point point, Map map, int min = -1, int max = 1)
		{
			var range = Enumerable .Range(min, max - min + 1);

			return range
				.SelectMany(x => range.Select(y => new Point(point.X + x, point.Y + y)))
				.Where(p => p != point && ((p.X - point.X) == 0 || (p.Y - point.Y) == 0) &&
					   map.InBounds(p) && map.Dungeon[p.X, p.Y] == MapCell.Empty);
		}
	}
}