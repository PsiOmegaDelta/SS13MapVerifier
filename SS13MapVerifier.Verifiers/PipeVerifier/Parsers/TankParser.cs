using System;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Verifiers.PipeVerifier.Parsers
{
    internal class TankParser : SectionParser
    {
        private const string CanParseType = "/obj/machinery/atmospherics/pipe/tank";

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseType);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var direction = this.GetDirection(atom, (Directions)2);
            return Tuple.Create(direction, direction, SectionType.Tank, ContentType.Any);
        }
    }
}
