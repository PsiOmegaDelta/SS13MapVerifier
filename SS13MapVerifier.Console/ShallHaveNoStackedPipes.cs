using System;
using System.Collections.Generic;
using System.Linq;

using SS13MapVerifier.Console.PipeVerifier;
using SS13MapVerifier.Console.PipeVerifier.Parsers;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console
{
    internal class ShallHaveNoStackedPipes : IVerifier
    {
        #region Public Methods and Operators

        private readonly CompleteSectionParser sectionParser = new CompleteSectionParser();

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            foreach (var tile in map.Tiles)
            {
                var visitedDirections = Directions.None;
                foreach (var atom in tile.Atoms.Where(x => sectionParser.CanParse(x)))
                {
                    var result = sectionParser.Parse(atom);
                    var currentDirections = result.Item1 | result.Item2;
                    if ((currentDirections & visitedDirections) != 0)
                    {
                        yield return new Log("Stacked pipes", Severity.Error, tile);
                        break;
                    }

                    visitedDirections |= currentDirections;
                }
            }
        }

        #endregion

        private class CompleteSectionParser : SectionParser
        {
            private readonly IEnumerable<SectionParser> allParsers = new List<SectionParser>
                                                                         {
                                                                             new BinaryParser(), 
                                                                             new ManifoldParser(), 
                                                                             new PipeParser(), 
                                                                             new PortablesConnectorParser(),
                                                                             new TankParser(),
                                                                             new TrinaryParser(),
                                                                             new UnaryParser(),
                                                                             new ValveParser()
                                                                         };

            public override bool CanParse(Atom atom)
            {
                return allParsers.SingleOrDefault(x => x.CanParse(atom)) != null;
            }

            public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
            {
                return allParsers.Single(x => x.CanParse(atom)).Parse(atom);
            }
        }
    }
}
