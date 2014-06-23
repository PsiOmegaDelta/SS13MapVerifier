using System;
using System.Collections.Generic;

namespace SS13MapVerifier.Map
{
    public interface ITile : IEquatable<ITile>
    {
        #region Public Properties

        Coordinate Coordinate { get; }

        IEnumerable<Atom> Atoms { get; }

        #endregion

        #region Public Methods and Operators

        ITile GetNeighbour(Direction direction);

        #endregion
    }
}