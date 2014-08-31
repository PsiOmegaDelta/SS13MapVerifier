using System.Collections.Generic;
using System.Diagnostics; 

using Common.Extensions;

namespace SS13MapVerifier.Map
{
    [DebuggerDisplay("{Coordinate.X} - {Coordinate.Y} - {Coordinate.Z}")]
    public class Tile : ITile
    {
        #region Fields
        
        private readonly Coordinate coordinate;

        private readonly IDictionary<Directions, ITile> neighbours = new Dictionary<Directions, ITile>();

        #endregion

        #region Constructors and Destructors

        public Tile(Coordinate coordinate, IEnumerable<Atom> atoms)
        {
            this.coordinate = coordinate;
            Atoms = atoms.ToArrayEfficient();
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

        public IEnumerable<Atom> Atoms { get; private set; }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            var tile = obj as ITile;
            return tile != null && this.Equals(tile);
        }

        // Assumption: One tile per coordinate
        public bool Equals(ITile other)
        {
            return this.Coordinate.Equals(other.Coordinate);
        }

        public override int GetHashCode()
        {
            return this.Coordinate.GetHashCode();
        }

        public ITile GetNeighbour(Directions direction)
        {
            ITile tile;
            this.neighbours.TryGetValue(direction, out tile);
            return tile;
        }

        #endregion

        #region Methods

        internal void AddNeighbour(Directions direction, ITile tile)
        {
            this.neighbours.Add(direction, tile);
        }

        #endregion
    }
}