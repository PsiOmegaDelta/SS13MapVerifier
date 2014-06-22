using System;
using System.Collections.Generic;

using Common;

namespace SS13MapVerifier.Map
{
    public class Map : IMap
    {
        #region Fields

        private readonly IDictionary<Coordinate, Tile> map;

        #endregion

        #region Constructors and Destructors

        public Map(IEnumerable<Tile> tiles)
        {
            this.map = new Dictionary<Coordinate, Tile>();
            foreach (var tile in tiles)
            {
                this.map.Add(tile.Coordinate, tile);
                this.SetupNeighbours(tile);
            }
        }

        #endregion

        #region Public Properties

        public IEnumerable<ITile> Tiles
        {
            get
            {
                return this.map.Values;
            }
        }

        #endregion

        #region Public Methods and Operators

        public ITile GetNeighbour(ITile tile, Direction direction)
        {
            var neighbour = tile.GetNeighbour(direction);
            if (neighbour == null && (direction == Direction.Up || direction == Direction.Down))
            {
                Tile result;
                if (map.TryGetValue(GetNewCoordinates(direction, tile.Coordinate), out result))
                {
                    neighbour = result;
                }
            }

            return neighbour;
        }

        public ITile GetTileAtCoordinate(Coordinate coordinate)
        {
            Tile result;
            map.TryGetValue(coordinate, out result);
            return result;
        }

        #endregion

        #region Methods

        private static Coordinate GetNewCoordinates(Direction direction, Coordinate coordinate)
        {
            int newX = coordinate.X, newY = coordinate.Y, newZ = coordinate.Z;
            switch (direction)
            {
                case Direction.East:
                case Direction.West:
                    newX = direction == Direction.East ? newX + 1 : newX - 1;
                    break;
                case Direction.North:
                case Direction.South:
                    newY = direction == Direction.North ? newY + 1 : newY - 1;
                    break;
                case Direction.Up:
                case Direction.Down:
                    newZ = direction == Direction.Up ? newZ + 1 : newZ - 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }

            return new Coordinate(newX, newY, newZ);
        }

        private void SetupNeighbours(Tile tile)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                // If you want the neighbour setup to go 0.5-1 second faster, filter out Z-level connections as well
                // Just remember to do something about the pipe verification that can go between Z-levels.
                if (direction == Direction.None || direction == Direction.Any)
                {
                    continue;
                }

                var neighbour = this.AcquireNeighbour(tile, direction);
                if (neighbour == null)
                {
                    continue;
                }

                tile.AddNeighbour(direction, neighbour);
                neighbour.AddNeighbour(Directions.GetOppositeDirection(direction), tile);
            }
        }

        private Tile AcquireNeighbour(ITile tile, Direction direction)
        {
            Tile neighbourTile;
            var newCoords = GetNewCoordinates(direction, tile.Coordinate);
            this.map.TryGetValue(newCoords, out neighbourTile);
            return neighbourTile;
        }

        #endregion
    }

    public interface IMap
    {
        #region Public Properties

        IEnumerable<ITile> Tiles { get; }

        #endregion

        #region Public Methods and Operators

        ITile GetNeighbour(ITile tile, Direction direction);

        ITile GetTileAtCoordinate(Coordinate coordinate);

        #endregion
    }
}
