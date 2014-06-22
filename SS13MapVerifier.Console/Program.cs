﻿using System.Linq;

using SS13MapVerifier.Console.PipeVerifier;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Enter path to map");
            var map = MapParser.ParseFile(System.Console.ReadLine());
            var apc = new ShallBeOneAndOnlyOneApcInEachArea();
            var purity = new OnlyAllowPureAreaInstances();
            var pipe = new ThereShallBeOnlyBeTwoPipeLines();
            foreach (var log in pipe.ValidateMap(map).Concat(apc.ValidateMap(map).Concat(purity.ValidateMap(map))))
            {
                System.Console.WriteLine(log.Severity + " - " + log.Message + " - " + log.Tiles.First().Coordinate);
            }

            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }
    }
}