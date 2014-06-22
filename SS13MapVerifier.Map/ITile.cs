using System;
using System.Collections.Generic;

namespace SS13MapVerifier.Map
{
    public interface ITile : IEquatable<ITile>
    {
        #region Public Properties

        IEnumerable<string> Contents { get; }

        Coordinate Coordinate { get; }

        #endregion

        #region Public Methods and Operators

        ITile GetNeighbour(Direction direction);

        bool HasContent(string content);

        #endregion
    }
}