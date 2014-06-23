using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

            while (tileToSections.SelectMany(x => x.Value).ToArray().Any(y => !y.HasBeenVisited && y.ContentType.HasFlag(ContentType.Supply)))
            {
                var startSection = tileToSections.SelectMany(x => x.Value).First(x => !x.HasBeenVisited && x.ContentType.HasFlag(ContentType.Supply));
                VisitNeighbours(tileToSections, startSection, ContentType.Supply, Directions.Any);
            }

            var allPipes = tileToSections.SelectMany(x => x.Value).ToArray();
            var visitedPipes = allPipes.Where(x => x.HasBeenVisited).ToArray();
            var otherPipes = allPipes.Where(x => !x.HasBeenVisited).ToArray();

            var unconnectedPipes = allPipes.Where(x => !x.FullyConnected && ContentTypesIntersect(x.ContentType, ContentType.Supply)).ToArray();

            yield break;
        }

        private static void VisitNeighbours(IDictionary<ITile, IList<Section>> tileToSections, Section section, ContentType contentType, Directions direction)
        {
            // var directions = section.Directions.GetFlags().OfType<Directions>().Except(new[] { Directions.None, Directions.Any }).ToArray();
            // section.FullyConnected = directions.All(x => GetSectionsFromTile(tileToSections, section.Tile.GetNeighbour(x), x).Any(y => ContentTypesIntersect(section.ContentType, y.ContentType)));

            section.HasBeenVisited = true;
            foreach (var pipeDirection in section.Directions.GetFlags().OfType<Directions>().Except(new[] { Directions.None, Directions.Any }))
            {
                var neighbourTile = section.Tile.GetNeighbour(pipeDirection);
                foreach (var neighbour in GetSectionsFromTile(tileToSections, neighbourTile, pipeDirection).Where(x => ContentTypesIntersect(section.ContentType, x.ContentType)))
                {
                    section.ConnectedDirections |= pipeDirection;
                    if (!neighbour.HasBeenVisited && ContentTypesIntersect(neighbour.ContentType, contentType))
                    {
                        VisitNeighbours(tileToSections, neighbour, contentType, pipeDirection);
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

        #region Methods

        private static Directions GetManifoldDirections(Directions enumDirection)
        {
            return (Directions.North | Directions.East | Directions.South | Directions.West) ^ enumDirection;
        }

        private Directions AcquireRelevantDirections(string content, int direction)
        {
            var enumDirection = (Directions)direction;
            if (true)
            {
                enumDirection = enumDirection == Directions.None ? Directions.South : enumDirection;
                enumDirection = GetManifoldDirections(enumDirection);
                return enumDirection;
            }

            return enumDirection;
        }

        #endregion
    }
}
