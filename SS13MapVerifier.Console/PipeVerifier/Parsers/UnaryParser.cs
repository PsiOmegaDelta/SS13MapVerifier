using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier.Parsers
{
    internal class UnaryParser : SectionParser
    {
        private const string CanParseType = "/obj/machinery/atmospherics/unary/";

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseType);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var input = this.GetDirection(atom, (Directions)2);
            return Tuple.Create(input, Directions.None, SectionType.Unary, ContentType.Any);
        }
    }
}
