using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SS13MapVerifier.Map
{
    public static class MapParser
    {
        private static readonly Regex ContentRegex;

        private static readonly Regex ZLayerStart;

        private const string ZLayerEnd = "\"}";

        static MapParser()
        {
            ContentRegex = new Regex("^\"(.*)\" = \\((.*)\\)$");
            ZLayerStart = new Regex("^\\(1,1,(\\d+)\\) = {\\\"$");
        }

        public static IMap ParseFile(string path)
        {
            var contents = new Dictionary<string, IEnumerable<string>>();
            using (var file = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                ParseContents(file, contents);
                var tiles = ParseZLayers(file, contents).ToList();

                return new Map(tiles);
            }
        }

        private static void ParseContents(TextReader file, IDictionary<string, IEnumerable<string>> contents)
        {
            string line;
            var contentKeyLength = -1;
            while (!string.IsNullOrWhiteSpace(line = file.ReadLine()))
            {
                var content = ParseContentLine(line);
                if (contentKeyLength == -1)
                {
                    contentKeyLength = content.Item1.Length;
                }
                else if (content.Item1.Length != contentKeyLength)
                {
                    throw new InvalidDataException(string.Format("Found content with unexpected key length. Expected {0}, was {1}.", contentKeyLength, content.Item1.Length));
                }

                contents.Add(content.Item1, content.Item2);
            }
        }

        private enum ZLayerState
        {
            ReadingLayer,
            EndOfLayer
        }

        private static IEnumerable<Tile> ParseZLayers(TextReader file, IDictionary<string, IEnumerable<string>> contents)
        {
            var state = ZLayerState.EndOfLayer;
            string readLine;
            var currentZLayer = -1;
            var rows = new List<string>();
            while ((readLine = file.ReadLine()) != null)
            {
                if (state == ZLayerState.EndOfLayer)
                {
                    var reg = ZLayerStart.Match(readLine);
                    if (reg.Success)
                    {
                        currentZLayer = int.Parse(reg.Groups[1].Value);
                        state = ZLayerState.ReadingLayer;
                    }
                }
                else if (state == ZLayerState.ReadingLayer)
                {
                    if (ZLayerEnd.Equals(readLine))
                    {
                        foreach (var tile in ParseZLayer(rows, currentZLayer, contents))
                        {
                            yield return tile;
                        }

                        rows.Clear();
                        state = ZLayerState.EndOfLayer;
                    }
                    else
                    {
                        rows.Add(readLine);
                    }
                }
            }
        }

        private static IEnumerable<Tile> ParseZLayer(IList<string> rows, int currentZLayer, IDictionary<string, IEnumerable<string>> contents)
        {
            if (contents.Count == 0)
            {
                yield break;
            }

            var keyLength = contents.First().Key.Length;
            for (var inverseY = 0; inverseY < rows.Count; inverseY++)
            {
                for (var x = 0; x * keyLength < rows[inverseY].Length; x++ )
                {
                    var key = rows[inverseY].Substring(x * keyLength, keyLength);
                    var y = rows.Count - inverseY;
                    yield return new Tile(new Coordinate(x + 1, y, currentZLayer), contents[key]);
                }
            }
        }

        internal static Tuple<string, IEnumerable<string>> ParseContentLine(string content)
        {
            var reg = ContentRegex.Match(content);
            var two = reg.Groups[1].Value;
            var three = reg.Groups[2].Value;

            // Can probably use regex groups to pre-split on , but without an Internet connection I'm not going to bother looking up how
            return new Tuple<string, IEnumerable<string>>(two, three.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
