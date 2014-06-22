using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Test
{
    [TestClass]
    public class MapParserTest
    {
        private const string ContentLineOne = "\"a\" = (/turf/simulated/floor,/area/atmos)";
        private readonly Tuple<string, string[]> contentLineOneResult = new Tuple<string, string[]>("a", new[] { "/turf/simulated/floor", "/area/atmos" });
        private const string ContentLineTwo = "\"b\" = (/turf/simulated/floor/bluegrid,/area/AIsattele)";
        private readonly Tuple<string, string[]> contentLineTwoResult = new Tuple<string, string[]>("b", new[] { "/turf/simulated/floor/bluegrid", "/area/AIsattele" });

        [TestMethod]
        public void ShallBeAbleToParseLineOne()
        {
            ParseLine(ContentLineOne, contentLineOneResult.Item1, contentLineOneResult.Item2);
        }

        [TestMethod]
        public void ShallBeAbleToParseLineTwo()
        {
            ParseLine(ContentLineTwo, contentLineTwoResult.Item1, contentLineTwoResult.Item2);
        }

        [TestMethod]
        public void ShallBeAbleToParseBayStation12()
        {
            var map = MapParser.ParseFile(@"C:\Users\nick\Documents\Baystation12-Dev\maps\tgstation2.dmm");
            Assert.IsNotNull(map);
            var tileAtCoordinate = map.GetTileAtCoordinate(new Coordinate(127, 131, 3));
            tileAtCoordinate = map.GetTileAtCoordinate(new Coordinate(113, 61, 1));
        }

        private static void ParseLine(string contentLine, string expectedKey, string[] expectedValue)
        {
            var result = MapParser.ParseContentLine(contentLine);
            Assert.AreEqual(expectedKey, result.Item1);
            for (var i = 0; i < expectedValue.Length; i++)
            {
                Assert.AreEqual(expectedValue[i], result.Item2.ElementAt(i));
            }
        }
    }
}
