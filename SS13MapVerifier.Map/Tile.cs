using System;
using System.Collections.Generic;
using System.Diagnostics;

using Common.Extensions;

namespace SS13MapVerifier.Map
{
    [DebuggerDisplay("{Coordinate.X} - {Coordinate.Y} - {Coordinate.Z}")]
    public class Tile : ITile
    {
        #region Fields

        private readonly IList<Atom> atoms = new List<Atom>();

        private readonly Coordinate coordinate;

        private readonly IDictionary<Direction, ITile> neighbours = new Dictionary<Direction, ITile>();

        #endregion

        #region Constructors and Destructors

        public Tile(Coordinate coordinate, object settings)
        {
            this.coordinate = coordinate;
            throw new NotImplementedException("setting");
        }

        #endregion

        #region Public Properties

        public Coordinate Coordinate
        {
            get
            {
                return this.coordinate;
            }
        }

        public IEnumerable<Atom> Atoms
        {
            get
            {
                return atoms;
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
                   && this.Atoms.Equal(other.Atoms);
        }

        public override int GetHashCode()
        {
            return this.Coordinate.GetHashCode() ^ this.Atoms.GetHashCode();
        }

        public ITile GetNeighbour(Direction direction)
        {
            ITile tile;
            this.neighbours.TryGetValue(direction, out tile);
            return tile;
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