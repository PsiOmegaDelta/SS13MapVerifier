using System.Collections.Generic;
using System.Linq;

using Common.Extensions;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Verifiers
{
    public class OnlyAllowPureAreaInstances : IVerifier
    {
        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            var errors = new Dictionary<string, Log>();

            foreach (var tile in map.Tiles)
            {
                foreach (var atom in tile.Atoms.Where(x => x.Type.StartsWith("/area/") && x.Settings.Any()))
                {
                    var atomClosure = atom;
                    var error = errors.SafeGetValue(
                        atom.Type,
                        () => new Log("Impure area - " + atomClosure.Type, Severity.Error));
                    error.AddTile(tile);
                }
            }

            return errors.Values;
        }

        #endregion
    }
}