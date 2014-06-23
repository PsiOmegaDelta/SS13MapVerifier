using System.Collections.Generic;

namespace SS13MapVerifier.Map
{
    public interface IMap
    {
        #region Public Properties

        IEnumerable<ITile> Tiles { get; }

        #endregion

        #region Public Methods and Operators

        ITile GetNeighbour(ITile tile, Directions direction);

        ITile GetTileAtCoordinate(Coordinate coordinate);

        #endregion
    }
}