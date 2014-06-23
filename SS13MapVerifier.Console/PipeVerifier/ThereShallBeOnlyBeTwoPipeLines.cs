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
            foreach (var tile in map.Tiles)
            {
                foreach (var atom in tile.Atoms.Where(Section.IsSection))
                {
                    tileToSections.SafeGetValue(tile, () => new List<Section>()).Add(new Section(tile, atom));
                }
            }

            while (tileToSections.Any(x => x.Value.Any(y => !y.HasBeenVisited && y.ContentType.HasFlag(ContentType.Supply))))
            {
                var startPoint = tileToSections.First(x => x.Value.Any(y => !y.HasBeenVisited && y.ContentType == ContentType.Supply)).Value.First();
                VisitNeighbours(tileToSections, startPoint, ContentType.Supply, Direction.Any);
            }

            var allPipes = tileToSections.SelectMany(x => x.Value).ToArray();
            var visitedPipes = allPipes.Where(x => x.HasBeenVisited).ToArray();
            var otherPipes = allPipes.Where(x => !x.HasBeenVisited).ToArray();

            var unconnectedPipes = allPipes.Where(x => !x.FullyConnected).ToArray();

            yield break;
        }

        private static void VisitNeighbours(IDictionary<ITile, IList<Section>> tileToSections, Section section, ContentType contentType, Direction direction)
        {
            var directions = section.Directions.GetFlags().OfType<Direction>().Except(new[] { Direction.None, Direction.Any }).ToArray();
            section.HasBeenVisited = true;
            section.FullyConnected = directions.All(x => GetSectionsFromTile(tileToSections, section.Tile.GetNeighbour(x), x).Any(y => Asd(section.ContentType, y.ContentType)));
            
            foreach (var pipeDirection in directions)
            {
                var neighbourTile = section.Tile.GetNeighbour(pipeDirection);
                foreach (var neighbour in GetSectionsFromTile(tileToSections, neighbourTile, direction).Where(x => !x.HasBeenVisited && x.ContentType.HasFlag(contentType)))
                {
                    VisitNeighbours(tileToSections, neighbour, contentType, pipeDirection);
                }
            }
        }

        private static bool Asd(ContentType contentsOne, ContentType contentsTwo)
        {
            return contentsOne.GetFlags().Intersect(contentsTwo.GetFlags()).Any();
        }

        private static IEnumerable<Section> GetSectionsFromTile(
            IDictionary<ITile, IList<Section>> tileToSections,
            ITile tile,
            Direction direction)
        {
            if (tile == null)
            {
                return Enumerable.Empty<Section>();
            }

            return tileToSections[tile].Where(x => x.Directions.HasFlag(Directions.GetOppositeDirection(direction)));
        }

        #endregion

        #region Methods

        private static Direction GetManifoldDirections(Direction enumDirection)
        {
            return (Direction.North | Direction.East | Direction.South | Direction.West) ^ enumDirection;
        }

        private Direction AcquireRelevantDirections(string content, int direction)
        {
            var enumDirection = (Direction)direction;
            if (true)
            {
                enumDirection = enumDirection == Direction.None ? Direction.South : enumDirection;
                enumDirection = GetManifoldDirections(enumDirection);
                return enumDirection;
            }

            return enumDirection;
        }

        #endregion
    }
}
