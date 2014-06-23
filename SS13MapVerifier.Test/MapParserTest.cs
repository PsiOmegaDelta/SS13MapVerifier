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
        private const string ContentLineTwo = "\"b\" = (/turf/simulated/floor/bluegrid{name = \"Te\\\"st\"; icon_state = \"1-2\"},/turf/simulated/floor,/area/AIsattele{name = \"Another \\\" Test\"})";
        private const string ContentLineThree = "\"abc\" = (/obj/machinery/camera{c_tag = \"Bedroom\"; dir = 6; network = list(\"SS13\",\"Prison\")},/obj/machinery/atmospherics/unary/vent_pump{dir = 4; on = 1},/turf/simulated/floor,/area/security/prison)";
        private const string ContentLineFour = "\"d\" = (/obj/machinery/camera{c_tag = \"Medbay Port Corridor\"; dir = 8; network = list(\"SS13\")},/obj/structure/disposalpipe/segment,/obj/machinery/light_switch{pixel_x = 22; pixel_y = 0},/obj/machinery/atmospherics/pipe/manifold/hidden/scrubbers{dir = 4},/turf/simulated/floor{dir = 4; icon_state = \"whiteblue\"; tag = \"icon-whitehall (WEST)\"},/area/medical/medbay2)";

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

        [TestMethod]
        public void ShallBeAbleToParseLineThree()
        {
            ParseLine(ContentLineThree, this.contentLineTwoResult.Item1, this.contentLineTwoResult.Item2);
        }

        [TestMethod]
        public void ShallBeAbleToParseLineFour()
        {
            ParseLine(ContentLineFour, this.contentLineTwoResult.Item1, this.contentLineTwoResult.Item2);
        }

        #endregion

        #region Methods

        private static void ParseLine(string contentLine, string expectedKey, string[] expectedValue)
        {
            var result = MapParser.ParseContentLine(contentLine);
            Assert.AreEqual(expectedKey, result.Item1);
            for (var i = 0; i < expectedValue.Length; i++)
            {
                // Assert.AreEqual(expectedValue[i], result.Item2.ElementAt(i));
            }
        }

        #endregion
    }
}