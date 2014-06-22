using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.Extensions;

namespace SS13MapVerifier.Map
{
    [DebuggerDisplay("{Coordinate.X} - {Coordinate.Y} - {Coordinate.Z}")]
    public class Tile : ITile
    {
        #region Fields

        private readonly IEnumerable<string> contents;

        private readonly Coordinate coordinate;

        private readonly IDictionary<Direction, ITile> neighbours = new Dictionary<Direction, ITile>();

        #endregion

        #region Constructors and Destructors

        public Tile(Coordinate coordinate, IEnumerable<string> contents)
        {
            this.coordinate = coordinate;
            this.contents = contents;
        }

        #endregion

        #region Public Properties

        public IEnumerable<string> Contents
        {
            get
            {
                return this.contents;
            }
        }

        public Coordinate Coordinate
        {
            get
            {
                return this.coordinate;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            var tile = obj as ITile;
            return tile != null && this.Equals(tile);
        }

        public bool Equals(ITile other)
        {
            return this.Coordinate.Equals(other.Coordinate)
                   && EnumerableExtensions.CompareTo(this.Contents, other.Contents) == 0;
        }

        public override int GetHashCode()
        {
            return this.Coordinate.GetHashCode() ^ this.Contents.GetHashCode();
        }

        public ITile GetNeighbour(Direction direction)
        {
            ITile tile;
            this.neighbours.TryGetValue(direction, out tile);
            return tile;
        }

        public bool HasContent(string content)
        {
            return this.Contents.Any(x => x.Equals(content));
        }

        #endregion

        #region Methods

        internal void AddNeighbour(Direction direction, ITile tile)
        {
            this.neighbours.Add(direction, tile);
        }

        #endregion
    }
}