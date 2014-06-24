using System;
using System.Collections.Generic;

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

        public ITile GetNeighbour(ITile tile, Directions direction)
        {
            var neighbour = tile.GetNeighbour(direction);
            if (neighbour == null && (direction == Directions.Up || direction == Directions.Down))
            {
                Tile result;
                if (this.map.TryGetValue(GetNewCoordinates(direction, tile.Coordinate), out result))
                {
                    neighbour = result;
                }
            }

            return neighbour;
        }

        public ITile GetTileAtCoordinate(Coordinate coordinate)
        {
            Tile result;
            this.map.TryGetValue(coordinate, out result);
            return result;
        }

        #endregion

        #region Methods

        private static Coordinate GetNewCoordinates(Directions direction, Coordinate coordinate)
        {
            int newX = coordinate.X, newY = coordinate.Y, newZ = coordinate.Z;
            switch (direction)
            {
                case Directions.East:
                case Directions.West:
                    newX = direction == Directions.East ? newX + 1 : newX - 1;
                    break;
                case Directions.North:
                case Directions.South:
                    newY = direction == Directions.North ? newY + 1 : newY - 1;
                    break;
                case Directions.Up:
                case Directions.Down:
                    newZ = direction == Directions.Up ? newZ + 1 : newZ - 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }

            return new Coordinate(newX, newY, newZ);
        }

        private Tile AcquireNeighbour(ITile tile, Directions direction)
        {
            Tile neighbourTile;
            var newCoords = GetNewCoordinates(direction, tile.Coordinate);
            this.map.TryGetValue(newCoords, out neighbourTile);
            return neighbourTile;
        }

        private void SetupNeighbours(Tile tile)
        {
            foreach (Directions direction in Enum.GetValues(typeof(Directions)))
            {
                // If you want the neighbour setup to go 0.5-1 second faster, filter out Z-level connections as well
                // Just remember to do something about the pipe verification that can go between Z-levels.
                if (direction == Directions.None || direction == Directions.Any || direction == Directions.Cardinal)
                {
                    continue;
                }

                var neighbour = this.AcquireNeighbour(tile, direction);
                if (neighbour == null)
                {
                    continue;
                }

                tile.AddNeighbour(direction, neighbour);
                neighbour.AddNeighbour(Direction.GetOppositeDirection(direction), tile);
            }
        }

        #endregion
    }
}