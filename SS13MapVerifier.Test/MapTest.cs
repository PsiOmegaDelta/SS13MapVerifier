using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Test
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void ShallBeAbleToGetEastNeighbourFromOneOne()
        {
            var tileOrigo = MakeTile(1, 1, 1);
            var tileExpected = MakeTile(2, 1, 1);
            var map = new Map.Map(new[] { tileOrigo, tileExpected, MakeTile(1, 2, 1), MakeTile(2, 2, 1) });

            Assert.AreEqual(tileExpected, map.GetNeighbour(tileOrigo, Directions.East));
        }

        [TestMethod]
        public void ShallBeAbleToGetNorthNeighbourFromOneOne()
        {
            var tileOrigo = MakeTile(1, 1, 1);
            var tileExpected = MakeTile(1, 2, 1);
            var map = new Map.Map(new[] { tileOrigo, tileExpected, MakeTile(2, 1, 1), MakeTile(2, 2, 1) });
            Assert.AreEqual(tileExpected, map.GetNeighbour(tileOrigo, Directions.North));
        }

        [TestMethod]
        public void ShallGetNullWhenRetrievingWestNeighbourFromOneOne()
        {
            var tileOrigo = MakeTile(1, 1, 1);
            var map = new Map.Map(new[] { tileOrigo, MakeTile(1, 2, 1), MakeTile(2, 1, 1), MakeTile(2, 2, 1) });
            Assert.IsNull(map.GetNeighbour(tileOrigo, Directions.West));
        }

        [TestMethod]
        public void ShallGetNullWhenRetrievingSouthNeighbourFromOneOne()
        {
            var tileOrigo = MakeTile(1, 1, 1);
            var map = new Map.Map(new[] { tileOrigo, MakeTile(1, 2, 1), MakeTile(2, 1, 1), MakeTile(2, 2, 1) });
            Assert.IsNull(map.GetNeighbour(tileOrigo, Directions.South));
        }

        [TestMethod]
        public void ShallBeAbleToGetuPNeighbourFromOneOne()
        {
            var tileOrigo = MakeTile(1, 1, 1);
            var tileExpected = MakeTile(1, 1, 2);
            var map = new Map.Map(new[] { tileOrigo, tileExpected, MakeTile(1, 2, 1), MakeTile(2, 1, 1), MakeTile(2, 2, 1) });
            Assert.AreEqual(tileExpected, map.GetNeighbour(tileOrigo, Directions.Up));
        }

        [TestMethod]
        public void ShallBeAbleToGetDownNeighbourFromOneOne()
        {
            var tileOrigo = MakeTile(1, 1, 1);
            var tileExpected = MakeTile(1, 1, 0);
            var map = new Map.Map(new[] { tileOrigo, tileExpected, MakeTile(1, 2, 1), MakeTile(2, 1, 1), MakeTile(2, 2, 1) });
            Assert.AreEqual(tileExpected, map.GetNeighbour(tileOrigo, Directions.Down));
        }

        private static Tile MakeTile(int x, int y, int z)
        {
            return new Tile(new Coordinate(x, y, z), Enumerable.Empty<Atom>());
        }
    }
}
