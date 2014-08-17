using System;
using System.Collections.Generic;
using System.Linq;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.Verifiers
{
    internal class VentsShouldHaveProperDefaultSettings : IVerifier
    {
        public IEnumerable<Log> ValidateMap(IMap map)
        {
            foreach (var tile in map.Tiles)
            {
                foreach (var atom in tile.Atoms.Where(x => x.Type == Types.UnaryVent))
                {
                    foreach (var setting in new[] { Tuple.Create("external_pressure_bound", new[] { "101.325" }), Tuple.Create("internal_pressure_bound", new[] { "0" }), Tuple.Create("pressure_checks", new[] { "1" }) })
                    {
                        var startValue = atom.GetSetting(setting.Item1);
                        var defaultValue = atom.GetSetting(setting.Item1 + "_default");

                        if (setting.Item2.Contains(startValue) && string.IsNullOrWhiteSpace(defaultValue))
                        {
                            continue;
                        }

                        if (!startValue.Equals(defaultValue))
                        {
                            yield return new Log(string.Format("{0} does not equal the default. Start/Default values: {1}/{2}", setting.Item1, startValue, defaultValue), Severity.Warning, tile);
                        }
                    }
                }
            }
        }
    }
}
