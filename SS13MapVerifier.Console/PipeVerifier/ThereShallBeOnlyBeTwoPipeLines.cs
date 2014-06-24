using System.Collections.Generic;
using System.Linq;

using Common.Extensions;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier
{
    internal class ThereShallBeOnlyBeTwoPipeLines
    {
        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            var tileToSections = new Dictionary<ITile, IList<Section>>();
            foreach (var tile in map.Tiles.Where(x => x.Coordinate.Z == 1))
            {
                foreach (var atom in tile.Atoms.Where(Section.IsSection))
                {
                    tileToSections.SafeGetValue(tile, () => new List<Section>()).Add(new Section(tile, atom));
                }
            }

            var visitedSupplyections = new HashSet<Section>();
            var connectedDirectionsSupply = new Dictionary<Section, Directions>();
            var supplySections = new List<Section>();
            while (tileToSections.SelectMany(x => x.Value).ToArray().Any(y => !visitedSupplyections.Contains(y) && y.ContentType == ContentType.Supply))
            {
                var startSection = tileToSections.SelectMany(x => x.Value).First(x => !visitedSupplyections.Contains(x) && x.ContentType == ContentType.Supply);
                supplySections.Add(startSection);
                VisitNeighbours(tileToSections, startSection, ContentType.Supply, visitedSupplyections, connectedDirectionsSupply);
            }

            var allPipes = tileToSections.SelectMany(x => x.Value).ToArray();
            var visitedPipes = allPipes.Where(visitedSupplyections.Contains).ToArray();
            var otherPipes = allPipes.Where(x => !visitedSupplyections.Contains(x)).ToArray();

            var unconnectedPipes = allPipes.Where(x => x.ContentType == ContentType.Supply && (!connectedDirectionsSupply.ContainsKey(x) || ((connectedDirectionsSupply[x] ^ x.Directions) != 0))).ToArray();

            yield break;
        }

        private static bool IsSupplyOrScrubber(ContentType contentType)
        {
            return contentType == ContentType.Scrubbers || contentType == ContentType.Supply;
        }

        private static void VisitNeighbours(IDictionary<ITile, IList<Section>> tileToSections, Section section, ContentType contentType, ISet<Section> visitedSections, IDictionary<Section, Directions> connectedDirections)
        {
            visitedSections.Add(section);
            foreach (var pipeDirection in section.Directions.GetFlags().OfType<Directions>().Except(new[] { Directions.None, Directions.Cardinal, Directions.Any }))
            {
                var neighbourTile = section.Tile.GetNeighbour(pipeDirection);
                foreach (var neighbour in GetSectionsFromTile(tileToSections, neighbourTile, pipeDirection).Where(x => ContentTypesIntersect(section.ContentType, x.ContentType)))
                {
                    connectedDirections[section] = connectedDirections.SafeGetValue(section, () => Directions.None) | pipeDirection;
                    if (!visitedSections.Contains(neighbour) && ContentTypesIntersect(neighbour.ContentType, contentType))
                    {
                        VisitNeighbours(tileToSections, neighbour, contentType, visitedSections, connectedDirections);
                    }
                }
            }
        }

        private static bool ContentTypesIntersect(ContentType contentsOne, ContentType contentsTwo)
        {
            return (contentsOne & contentsTwo) > 0;
        }

        private static bool DirectionsIntersect(Directions directionsOne, Directions directionsTwo)
        {
            return (directionsOne & directionsTwo) > 0;
        }

        private static IEnumerable<Section> GetSectionsFromTile(
            IDictionary<ITile, IList<Section>> tileToSections,
            ITile tile,
            Directions direction)
        {
            if (tile == null || !tileToSections.ContainsKey(tile))
            {
                return Enumerable.Empty<Section>();
            }

            var sections = tileToSections[tile].Where(x => DirectionsIntersect(x.Directions, Direction.GetOppositeDirection(direction)));
            return sections;
        }

        #endregion
    }
}
