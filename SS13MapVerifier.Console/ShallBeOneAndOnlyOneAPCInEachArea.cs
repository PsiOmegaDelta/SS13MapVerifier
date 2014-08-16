using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Common.Extensions;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console
{
    internal class ShallBeOneAndOnlyOneApcInEachArea : IVerifier
    {
        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            var apcCount = new Dictionary<string, ApcCounter>();

            foreach (var tile in map.Tiles.Where(x => x.Coordinate.Z == 1 || x.Coordinate.Z == 5))
            {
                // We do not care for space or areas in space. 
                // Assumption: Only one turf per tile
                if (tile.Atoms.Any(x => "/turf/space".Equals(x.Type)) || tile.Atoms.Any(x => "/area".Equals(x.Type))
                    || tile.Atoms.Any(x => x.Type.StartsWith("/area/holodeck/"))
                    || tile.Atoms.Any(x => x.Type.StartsWith("/area/shuttle/"))
                    || tile.Atoms.Any(x => x.Type.StartsWith("/area/solar/"))
                    || tile.Atoms.Any(x => x.Type.Equals("/area/mine/unexplored"))
                    || tile.Atoms.Any(x => x.Type.Equals("/area/mine/abandoned")))
                {
                    continue;
                }

                var area = tile.Atoms.FirstOrDefault(x => x.Type.StartsWith("/area/"));
                if (area == null)
                {
                    var log = new Log("Non-space turf without non-space area", Severity.Error);
                    log.AddTile(tile);
                    yield return log;
                    yield break;
                }

                var numberOfApcs =
                    tile.Atoms.Count(
                        x => x.Type.Equals("/obj/machinery/power/apc") || x.Type.StartsWith("/obj/machinery/power/apc{"));
                var counter = apcCount.SafeGetValue(area.Type, () => new ApcCounter());
                counter.Count += numberOfApcs;
                counter.Tiles.Add(tile);
            }

            foreach (var badArea in apcCount.Where(x => x.Value.Count != 1))
            {
                var log = new Log(
                    string.Format("Bad APC count: {0} - {1}", badArea.Key, badArea.Value.Count), 
                    Severity.Error);
                foreach (var tile in badArea.Value.Tiles)
                {
                    log.AddTile(tile);
                }

                yield return log;
            }
        }

        #endregion

        [DebuggerDisplay("{Count}")]
        private class ApcCounter
        {
            #region Fields

            private readonly IList<ITile> tiles = new List<ITile>();

            #endregion

            #region Public Properties

            public int Count { get; set; }

            public IList<ITile> Tiles
            {
                get
                {
                    return this.tiles;
                }
            }

            #endregion
        }
    }
}