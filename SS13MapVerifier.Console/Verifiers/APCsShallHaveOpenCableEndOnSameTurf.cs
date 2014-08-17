using System.Collections.Generic;
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
                        if (atom.Settings.Any(x => x.Key == "dir"))
                        {
                            var dir = (Directions)int.Parse(atom.Settings["dir"].Single());
                            if (dir == Directions.North || dir == Directions.East || dir == Directions.South
                                || dir == Directions.West)
                            {
                                hasOneDirectionCable = true;
                            }
                        }
                        else
                        {
                            // No direction set implies dir = 2
                            hasOneDirectionCable = true;
                        }
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
