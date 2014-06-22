using System;
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

        public bool HasContent(string content)
        {
            return Contents.Any(x => x.Equals(content));
        }

        public Coordinate Coordinate
        {
            get
            {
                return this.coordinate;
            }
        }

        public ITile GetNeighbour(Direction direction)
        {
            ITile tile;
            neighbours.TryGetValue(direction, out tile);
            return tile;
        }

        internal void AddNeighbour(Direction direction, ITile tile)
        {
            neighbours.Add(direction, tile);
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            var tile = obj as ITile;
            return tile != null && Equals(tile);
        }

        public bool Equals(ITile other)
        {
            return Coordinate.Equals(other.Coordinate) && EnumerableExtensions.CompareTo(this.Contents, other.Contents) == 0;
        }

        public override int GetHashCode()
        {
            return Coordinate.GetHashCode() ^ this.Contents.GetHashCode();
        }

        #endregion
    }

    public interface ITile : IEquatable<ITile>
    {
        IEnumerable<string> Contents { get; }

        bool HasContent(string content);

        Coordinate Coordinate { get; }

        ITile GetNeighbour(Direction direction);
    }
}
