using System.Collections.Generic;
using System.Linq;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.Verifiers
{
    internal class ShallHaveNoStackedPowerCables : IVerifier
    {
        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            foreach (var tile in map.Tiles)
            {
                var groups = tile.Atoms.Where(x => x.Type == Types.PowerCable).GroupBy(x => x.GetSetting("d1") + x.GetSetting("d2"));
                if (groups.Any(x => x.Count() > 1))
                {
                    yield return new Log("Stacked power cables", Severity.Error, tile);
                }
            }
        }

        #endregion
    }
}
