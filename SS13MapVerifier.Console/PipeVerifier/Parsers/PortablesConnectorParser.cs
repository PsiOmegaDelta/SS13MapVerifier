using System;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier.Parsers
{
    internal class PortablesConnectorParser : SectionParser
    {
        private const string CanParseType = "/obj/machinery/atmospherics/portables_connector";

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseType);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var direction = this.GetDirection(atom, (Directions)2);
            return Tuple.Create(direction, Directions.None, SectionType.PortablesConnector, ContentType.Any);
        }
    }
}
