using System;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Verifiers.PipeVerifier.Parsers
{
    internal class TrinaryParser : SectionParser
    {
        private const string CanParseTrinary = "/obj/machinery/atmospherics/trinary/";
        private const string Filter = "/obj/machinery/atmospherics/trinary/filter";

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseTrinary);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var isFilter = atom.Type.StartsWith(Filter);
            var directions = isFilter ? this.GetFilterDirection(atom) : this.GetMixerDirection(atom);

            // TODO: Consider mapping content type per input/output direction.
            return Tuple.Create(directions.Item1, directions.Item2, isFilter ? SectionType.Filter : SectionType.Mixer, ContentType.Any);
        }

        private Tuple<Directions, Directions> GetMixerDirection(Atom atom)
        {
            var output = this.GetDirection(atom, (Directions)2);
            Directions input;
            if (atom.Type.Contains("/m_"))
            {
                input = Direction.GetOppositeDirection(output);
                input |= Direction.GetDirection90DegreesClockWise(input);
            }
            else if (atom.Type.Contains("/t_"))
            {
                input = Direction.GetDirection90DegreesClockWise(output) | Direction.GetDirection90DegreesCounterClockWise(output);
            }
            else
            {
                input = Direction.GetOppositeDirection(output);
                input |= Direction.GetDirection90DegreesCounterClockWise(input);
            }

            return Tuple.Create(input, output);
        }

        private Tuple<Directions, Directions> GetFilterDirection(Atom atom)
        {
            var output = this.GetDirection(atom, (Directions)2);
            var input = Direction.GetOppositeDirection(output);
            if (atom.Type.Contains("/m_"))
            {
                output |= Direction.GetDirection90DegreesCounterClockWise(output);
            }
            else
            {
                output |= Direction.GetDirection90DegreesClockWise(output);
            }

            return Tuple.Create(input, output);
        }
    }
}
