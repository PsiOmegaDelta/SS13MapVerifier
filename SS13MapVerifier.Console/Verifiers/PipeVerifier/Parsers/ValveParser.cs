using System;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.Verifiers.PipeVerifier.Parsers
{
    internal class ValveParser : SectionParser
    {
        private const string CanParseValve = "/obj/machinery/atmospherics/valve";
        private const string CanParseTValve = "/obj/machinery/atmospherics/tvalve";

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseValve) || atom.Type.StartsWith(CanParseTValve);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var isValve = atom.Type.StartsWith(CanParseValve);

            var output = this.GetDirection(atom, (Directions)2);
            var input = Direction.GetOppositeDirection(output);
            if (isValve)
            {
                output |= input;
                input |= output;
            }
            else
            {
                output |= atom.Type.Contains("mirrored") 
                    ? Direction.GetDirection90DegreesCounterClockWise(output)
                    : Direction.GetDirection90DegreesClockWise(output);
            }

            return Tuple.Create(input, output, isValve ? SectionType.Valve : SectionType.TValve, ContentType.Any);
        }
    }
}
