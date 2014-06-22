using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Test
{
    [TestClass]
    public class MapParserTest
    {
        #region Constants

        private const string ContentLineOne = "\"a\" = (/turf/simulated/floor,/area/atmos)";
        private const string ContentLineTwo = "\"b\" = (/turf/simulated/floor/bluegrid,/area/AIsattele)";

        #endregion

        #region Fields
        
        private readonly Tuple<string, string[]> contentLineOneResult = new Tuple<string, string[]>("a", new[] { "/turf/simulated/floor", "/area/atmos" });
        private readonly Tuple<string, string[]> contentLineTwoResult = new Tuple<string, string[]>("b", new[] { "/turf/simulated/floor/bluegrid", "/area/AIsattele" });

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void ShallBeAbleToParseBayStation12()
        {
            var map = MapParser.ParseFile(@"C:\Users\nick\Documents\Baystation12-Dev\maps\tgstation2.dmm");
            Assert.IsNotNull(map);
            var tileAtCoordinate = map.GetTileAtCoordinate(new Coordinate(127, 131, 3));
            tileAtCoordinate = map.GetTileAtCoordinate(new Coordinate(113, 61, 1));
        }

        [TestMethod]
        public void ShallBeAbleToParseLineOne()
        {
            ParseLine(ContentLineOne, this.contentLineOneResult.Item1, this.contentLineOneResult.Item2);
        }

        [TestMethod]
        public void ShallBeAbleToParseLineTwo()
        {
            ParseLine(ContentLineTwo, this.contentLineTwoResult.Item1, this.contentLineTwoResult.Item2);
        }

        #endregion

        #region Methods

        private static void ParseLine(string contentLine, string expectedKey, string[] expectedValue)
        {
            var result = MapParser.ParseContentLine(contentLine);
            Assert.AreEqual(expectedKey, result.Item1);
            for (var i = 0; i < expectedValue.Length; i++)
            {
                Assert.AreEqual(expectedValue[i], result.Item2.ElementAt(i));
            }
        }

        #endregion
    }
}