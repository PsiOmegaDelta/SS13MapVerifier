using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Common.Extensions;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console
{
    internal class ShallBeOneAndOnlyOneApcInEachArea
    {
        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            var apcCount = new Dictionary<string, ApcCounter>();

            foreach (var tile in map.Tiles.Where(x => x.Coordinate.Z == 1 || x.Coordinate.Z == 5))
            {
                // We do not care for space or areas in space. 
                // Assumption: Only one turf per tile
                if (tile.Contents.Any(x => "/turf/space".Equals(x)) || tile.Contents.Any(x => "/area".Equals(x))
                    || tile.Contents.Any(x => x.StartsWith("/area/holodeck/"))
                    || tile.Contents.Any(x => x.StartsWith("/area/shuttle/"))
                    || tile.Contents.Any(x => x.StartsWith("/area/solar/"))
                    || tile.Contents.Any(x => x.Equals("/area/mine/unexplored"))
                    || tile.Contents.Any(x => x.Equals("/area/mine/abandoned")))
                {
                    continue;
                }

                var areaName = tile.Contents.FirstOrDefault(x => x.StartsWith("/area/"));
                if (areaName == null)
                {
                    var log = new Log("Non-space tile without non-space area", Severity.Error);
                    log.AddTile(tile);
                    yield return log;
                }

                var numberOfApcs =
                    tile.Contents.Count(
                        x => x.Equals("/obj/machinery/power/apc") || x.StartsWith("/obj/machinery/power/apc{"));
                var counter = apcCount.SafeGetValue(areaName, () => new ApcCounter());
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