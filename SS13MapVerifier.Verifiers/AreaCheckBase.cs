using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Common.Extensions;

using SS13MapVerifier.Map;
using SS13MapVerifier.Map.Constants;

namespace SS13MapVerifier.Verifiers
{
    public abstract class AreaCheckBase : IVerifier
    {
        #region Properties

        protected abstract Func<int, bool> BadCountPredicate { get; }

        protected abstract string CheckedType { get; }

        protected abstract IEnumerable<int> CheckedZLevels { get; }

        protected abstract string ErrorMessage { get; }

        protected abstract IEnumerable<string> IgnoredAreas { get; }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            var checkedTypeCount = new Dictionary<string, SupportCounter>();
            foreach (var tile in map.Tiles.Where(x => this.CheckedZLevels.Contains(x.Coordinate.Z)))
            {
                if (Enumerable.Any<string>(tile.Atoms.SelectMany(atom => this.IgnoredAreas.Where(area => atom.Type.IsType(area)))))
                {
                    continue;
                }

                var areaType = tile.Atoms.Single(x => x.Type.IsType(Areas.Area)).Type;
                var numberOfTheCheckedType = tile.Atoms.Count(x => x.Type.IsType(this.CheckedType));
                var counter = checkedTypeCount.SafeGetValue(areaType, () => new SupportCounter());
                counter.Count += numberOfTheCheckedType;
                counter.Tiles.Add(tile);
            }

            foreach (var badArea in checkedTypeCount.Where(x => this.BadCountPredicate(x.Value.Count)))
            {
                var log = new Log(
                    string.Format("{0}: {1} - {2}", this.ErrorMessage, badArea.Key, badArea.Value.Count), 
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
        private class SupportCounter
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
