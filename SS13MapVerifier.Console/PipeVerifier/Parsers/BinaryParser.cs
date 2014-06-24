using System;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier.Parsers
{
    internal class BinaryParser : SectionParser
    {
        private const string CanParseType = "/obj/machinery/atmospherics/binary";

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseType);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var output = this.GetDirection(atom, (Directions)2);
            var input = Direction.GetOppositeDirection(output);
            return Tuple.Create(input, output, SectionType.Binary, ContentType.Any);
        }
    }
}
