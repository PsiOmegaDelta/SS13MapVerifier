using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Extensions;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Verifiers.PipeVerifier
{
    public class ThereShallBeOnlyBeTwoPipeLines : IVerifier
    {
        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            var tileToSections = new Dictionary<ITile, IList<Section>>();
            foreach (var tile in map.Tiles)
            {
                foreach (var atom in tile.Atoms.Where(Section.IsSection))
                {
                    tileToSections.SafeGetValue(tile, () => new List<Section>()).Add(new Section(tile, atom));
                }
            }

            var visitedSections = new HashSet<Section>();
            var connectedDirections = new Dictionary<Section, Directions>();
            var sections = new Dictionary<ContentType, List<Section>>();

            foreach (var contentType in new[] { ContentType.Cyan, ContentType.Green, ContentType.Scrubbers, ContentType.Supply, ContentType.Yellow, ContentType.Any })
            {
                RunChecks(
                    tileToSections,
                    visitedSections,
                    sections.SafeGetValue(contentType, () => new List<Section>()),
                    connectedDirections,
                    contentType);
            }

            var allPipes = tileToSections.SelectMany(x => x.Value);
            var unconnectedPipes = allPipes.Where(x => !connectedDirections.ContainsKey(x) || ((connectedDirections[x] ^ x.Directions) != 0));

            foreach (var unconnectedPipe in unconnectedPipes.OrderBy(x => x.ContentType))
            {
                yield return new Log(string.Format("Unconnected pipe - {0}", unconnectedPipe.ContentType), Severity.Error, unconnectedPipe.Tile);
            }

            var supplyGroups = sections[ContentType.Supply].GroupBy(x => x.Tile.Coordinate.Z).ToArray();
            foreach (var supplyGroup in supplyGroups.Where(x => x.Count() > 1))
            {
                foreach (var section in supplyGroup)
                {
                    yield return new Log("Separate supply sections", Severity.Error, section.Tile);   
                }
            }

            var scrubberGroups = sections[ContentType.Scrubbers].GroupBy(x => x.Tile.Coordinate.Z).ToArray();
            foreach (var scrubberSection in scrubberGroups.Where(x => x.Count() > 1))
            {
                foreach (var section in scrubberSection)
                {
                    yield return new Log("Separate scrubbers sections", Severity.Error, section.Tile);                    
                }
            }
        }

        private static void RunChecks(
            IDictionary<ITile, IList<Section>> tileToSections,
            ISet<Section> visitedSections,
            List<Section> separateSections,
            IDictionary<Section, Directions> connectedDirections,
            ContentType contentType)
        {
            if (separateSections == null)
            {
                throw new ArgumentNullException("separateSections");
            }

            while (tileToSections.SelectMany(x => x.Value)
                    .ToArray()
                    .Any(y => !visitedSections.Contains(y) && y.ContentType == contentType))
            {
                var startSection =
                    tileToSections.SelectMany(x => x.Value)
                        .First(x => !visitedSections.Contains(x) && x.ContentType == contentType);
                separateSections.Add(startSection);
                VisitNeighbours(
                    tileToSections,
                    startSection,
                    contentType,
                    visitedSections,
                    connectedDirections);
            }
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
