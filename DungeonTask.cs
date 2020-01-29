using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
	public class DungeonTask
	{
		public static MoveDirection[] FindShortestPath(Map map)
		{
			var pathsToChest = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
			var dictChest = pathsToChest.ToDictionary(way => way.Value, way => way.Count());

			var pathsToExit = BfsTask.FindPaths(map, map.Exit, pathsToChest.Count() > 0 ? dictChest.Keys.ToArray() : new[] { map.InitialPosition });
			if (pathsToExit.Count() == 0) return new MoveDirection[0];
			var dictExit = pathsToExit.ToDictionary(way => way.Value, way => way.Count());

			var bestWay = dictChest.OrderBy(w => w.Value + dictExit[w.Key]).FirstOrDefault().Key;

			var pathChest = pathsToChest.FirstOrDefault(p => p.Value == bestWay);
			var pathExit = pathsToExit.FirstOrDefault(p => p.Value == bestWay);

			return pathChest == null ?
					GetMovesByWay(pathExit).ToArray() :
					GetMovesByWay(pathChest.Reverse()).Concat(GetMovesByWay(pathExit)).ToArray();
		}

		public static IEnumerable<MoveDirection> GetMovesByWay(IEnumerable<Point> way)
		{
			var prev = way.FirstOrDefault();
			foreach (var cur in way.Skip(1))
			{
				int dx = cur.X - prev.X;
				int dy = cur.Y - prev.Y;
				yield return dx != 0 ? 
							 dx > 0 ? MoveDirection.Right : MoveDirection.Left :
							 dy > 0 ? MoveDirection.Down : MoveDirection.Up;
				prev = cur;
			}
		}
	}
}