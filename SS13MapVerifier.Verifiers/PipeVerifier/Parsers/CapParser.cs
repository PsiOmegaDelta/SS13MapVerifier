using System;

using SS13MapVerifier.Map;
using SS13MapVerifier.Map.Constants;

namespace SS13MapVerifier.Verifiers.PipeVerifier.Parsers
{
    internal class CapParser : SectionParser
    {
        private const string CanParseType = Objects.PipeCap;

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseType);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var directions = this.GetDirection(atom, (Directions)2);
            return Tuple.Create(directions, directions, SectionType.Binary, ContentType.Any);
        }
    }
}
