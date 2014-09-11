using System;
using System.Collections.Generic;
using System.Linq;

using Common.Extensions;

using SS13MapVerifier.Map;
using SS13MapVerifier.Verifiers.PipeVerifier;
using SS13MapVerifier.Verifiers.PipeVerifier.Parsers;

namespace SS13MapVerifier.Verifiers
{
    public class ShallHaveNoStackedPipes : IVerifier
    {
        #region Fields

        private readonly CompleteSectionParser sectionParser = new CompleteSectionParser();

        #endregion

        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            foreach (var tile in map.Tiles)
            {
                var visitedSupplyDirections = new Dictionary<ContentType, Directions>();
                var visitedScrubberDirections = new Dictionary<ContentType, Directions>();
                foreach (var atom in tile.Atoms.Where(x => this.sectionParser.CanParse(x)))
                {
                    var result = this.sectionParser.Parse(atom);
                    var currentDirections = result.Item1 | result.Item2;
                    var contentType = SupplyScrubberOrAnyType(result.Item4);

                    if (contentType != ContentType.Scrubbers)
                    {
                        if ((currentDirections & visitedSupplyDirections.SafeGetValue(contentType, () => Directions.None)) != 0)
                        {
                            yield return new Log("Stacked pipes", Severity.Error, tile);
                            break;
                        }

                        visitedSupplyDirections[contentType] |= currentDirections;
                    }

                    if (contentType != ContentType.Supply)
                    {
                        if ((currentDirections & visitedScrubberDirections.SafeGetValue(contentType, () => Directions.None)) != 0)
                        {
                            yield return new Log("Stacked pipes", Severity.Error, tile);
                            break;
                        }

                        visitedScrubberDirections[contentType] |= currentDirections;
                    }
                }
            }
        }

        #endregion

        #region Methods

        private static ContentType SupplyScrubberOrAnyType(ContentType result)
        {
            if (result == ContentType.Scrubbers)
            {
                return ContentType.Scrubbers;
            }

            if (result == ContentType.Supply)
            {
                return ContentType.Supply;
            }

            return ContentType.Any;
        }

        #endregion

        private class CompleteSectionParser : SectionParser
        {
            #region Fields



            #endregion

            #region Public Methods and Operators

            public override bool CanParse(Atom atom)
            {
                return Section.Parsers.SingleOrDefault(x => x.CanParse(atom)) != null;
            }

            public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
            {
                return Section.Parsers.Single(x => x.CanParse(atom)).Parse(atom);
            }

            #endregion
        }
    }
}
