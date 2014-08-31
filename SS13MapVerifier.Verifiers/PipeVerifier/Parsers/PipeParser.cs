using System;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Verifiers.PipeVerifier.Parsers
{
    internal class PipeParser : SectionParser
    {
        #region Fields

        private const string CanParseType = "/obj/machinery/atmospherics/pipe/simple";

        #endregion

        #region Public Methods and Operators

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseType);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var directions = this.GetDirections(atom);
            var connectionType = GetContentType(atom);

            return Tuple.Create(directions, directions, SectionType.Pipe, connectionType);
        }

        #endregion

        #region Methods

        private static Directions GetAllDirections(Directions original, Directions either, Directions or)
        {
            if ((original ^ either) == 0)
            {
                original |= or;
            }
            else if ((original ^ or) == 0)
            {
                original |= either;
            }

            return original;
        }

        private Directions GetDirections(Atom atom)
        {
            var directions = this.GetDirection(atom, (Directions)2);
            return GetAllDirections(directions, Directions.North, Directions.South)
                   | GetAllDirections(directions, Directions.West, Directions.East);
        }

        #endregion
    }
}