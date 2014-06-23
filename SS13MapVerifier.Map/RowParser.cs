
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SS13MapVerifier.Map
{
    internal static class RowParser
    {
        // "/turf/space,/area"
        // /obj/machinery/camera{c_tag = "Bedroom"; dir = 6; network = list("SS13","Prison")},/obj/machinery/atmospherics/unary/vent_pump{dir = 4; on = 1},/turf/simulated/floor,/area/security/prison)
        public static IEnumerable<Atom> Parse(string row)
        {
            while (true)
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

                yield return new Atom(type, options.ToLookup(x => x.Item1, x => x.Item2));
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

        private static Tuple<string, IEnumerable<Tuple<string, string>>> ParseVars(string row)
        {
            if (string.IsNullOrEmpty(row) || row.Take(1).First() != '{')
            {
                return new Tuple<string, IEnumerable<Tuple<string, string>>>(row, Enumerable.Empty<Tuple<string, string>>());
            }

            var vars = new List<Tuple<string, string>>();
            row = row.Substring(1);

            do
            {
                var name = new string(row.SkipWhile(x => x == ' ' || x == ';').TakeWhile(x => x != ' ').ToArray());
                row = new string(row.SkipWhile(x => x != '=').Skip(2).ToArray());

                var value = string.Empty;
                if (row.StartsWith("\""))
                {
                    value = Regex.Match(row, @"""[^""\\]*(?:\\.[^""\\]*)*""").Value;
                }
                else if (row.StartsWith("list"))
                {
                    value = Regex.Match(row, @"^(list\([^\)]*\))").Value;
                }
                else
                {
                    value = new string(row.TakeWhile(x => x != ';' && x != '}').ToArray());
                }

                row = row.Substring(value.Length);
                vars.Add(Tuple.Create(name, value));

                if (string.IsNullOrEmpty(row))
                {
                }
            }
            while (!row.StartsWith("}"));

            row = new string(row.SkipWhile(x => x == '}' || x == ',').ToArray());
            return new Tuple<string, IEnumerable<Tuple<string, string>>>(row, vars);
        }
    }
}
