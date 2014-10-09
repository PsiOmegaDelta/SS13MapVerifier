using System.Collections.Generic;
using System.Linq;

using Common.Extensions;

using SS13MapVerifier.Map;
using SS13MapVerifier.Map.Constants;

namespace SS13MapVerifier.Verifiers
{
    public class ShallHaveUniqueCameraCTags : IVerifier
    {
        public IEnumerable<Log> ValidateMap(IMap map)
        {
            var cameraTiles = (from tile in map.Tiles from atom in tile.Atoms where atom.Type.IsType(Objects.Camera) select tile).ToList();
            var cameraAtoms = cameraTiles.SelectMany(x => x.Atoms).Where(x => x.Type.IsType(Objects.Camera));

            var cameraGroups = cameraAtoms.GroupBy(x => x.GetSetting("c_tag"));
            foreach (var badGroup in cameraGroups.Where(x => !string.IsNullOrWhiteSpace(x.Key) && x.Count() > 1))
            {
                var gropClosure = badGroup;
                var tiles = cameraTiles.Where(x => gropClosure.Any(y => x.Atoms.Contains(y)));
                yield return new Log("Duplicate c_tags", Severity.Error, tiles);
            }
        }
    }
}
