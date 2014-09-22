using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SS13MapVerifier.Map
{
    public interface IMap
    {
        #region Public Properties

        IEnumerable<ITile> Tiles { get; }

        ReadOnlyDictionary<int, Tuple<int, int>> MapSize { get; }

        #endregion

        #region Public Methods and Operators

        ITile GetNeighbour(ITile tile, Directions direction);

        ITile GetTileAtCoordinate(Coordinate coordinate);

        #endregion
    }
}