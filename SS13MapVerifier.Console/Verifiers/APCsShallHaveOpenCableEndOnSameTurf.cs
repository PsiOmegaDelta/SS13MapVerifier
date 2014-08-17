﻿using System.Collections.Generic;
using System.Linq;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.Verifiers
{
    internal class AreaPowerControlsShallHaveOpenCableEndOnSameTurf : IVerifier
    {
        public IEnumerable<Log> ValidateMap(IMap map)
        {
            foreach (var tile in map.Tiles)
            {
                var hasOneDirectionCable = false;
                var hasAPC = false;
                foreach (var atom in tile.Atoms)
                {
                    if (atom.Type == Types.APC)
                    {
                        hasAPC = true;
                    }

                    if (!hasOneDirectionCable && atom.Type == Types.PowerCable)
                    {
                        var dir1 = atom.GetSetting("d1", "0");
                        var dir2 = atom.GetSetting("d2", "1");
                        hasOneDirectionCable = (dir1 == "0" && dir2 != "0") || (dir1 != "0" && dir2 == "0");
                    }
                }

                if (hasAPC && !hasOneDirectionCable)
                {
                    yield return new Log("APC without power connection", Severity.Error, tile);
                }
            }
        }
    }
}
