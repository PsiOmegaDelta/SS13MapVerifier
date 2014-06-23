
using System;
using System.Collections.Generic;
using System.Linq;

namespace SS13MapVerifier.Map
{
    internal static class RowParser
    {
        // "/turf/space,/area"
        // /obj/machinery/camera{c_tag = "Bedroom"; dir = 6; network = list("SS13","Prison")},/obj/machinery/atmospherics/unary/vent_pump{dir = 4; on = 1},/turf/simulated/floor,/area/security/prison)
        public static IEnumerable<Tuple<string, IEnumerable<Tuple<string, string>>>> Parse(string row)
        {
            if (string.IsNullOrEmpty(row))
            {
                yield break;
            }

            var parsedType = ParseType(row);
            row = parsedType.Item1;
            var type = parsedType.Item2;

            var parsedVars = ParseVars(row);
            row = parsedVars.Item1;
            var options = parsedVars.Item2;

            yield return new Tuple<string, IEnumerable<Tuple<string, string>>>(type, options);
            foreach (var result in Parse(row))
            {
                yield return result;
            }
        }

        private static Tuple<string, string> ParseType(string row)
        {
            var type = new string(row.TakeWhile(x => x != '{' && x != ',').ToArray());
            if (type.Length < row.Length)
            {
                row = row.ElementAt(type.Length) == '{' ? row.Substring(type.Length) : row.Substring(type.Length + 1);
            }
            else
            {
                row = string.Empty;
            }

            return new Tuple<string, string>(row, type);
        }

        private static Tuple<string, IEnumerable<Tuple<string, string>>> ParseVars(string row, bool firstCall = true)
        {
            if (firstCall && row.Take(1).First() != '{')
            {
                return new Tuple<string, IEnumerable<Tuple<string, string>>>(row, Enumerable.Empty<Tuple<string, string>>());
            }

            throw new NotImplementedException();
        }
    }
}
