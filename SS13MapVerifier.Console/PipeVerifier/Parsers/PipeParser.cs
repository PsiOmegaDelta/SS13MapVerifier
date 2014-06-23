using System;
using System.Text.RegularExpressions;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier.Parsers
{
    internal class PipeParser : SectionParser
    {
        #region Fields

        private readonly Regex canParse = new Regex("^/obj/machinery/atmospherics/pipe/simple.*$");

        #endregion

        #region Public Methods and Operators

        public override bool CanParse(Atom atom)
        {
            return this.canParse.IsMatch(atom.Type);
        }

        public override Tuple<Direction, Direction, SectionType, ContentType> Parse(Atom atom)
        {
            var directions = this.GetDirections(atom);
            var connectionType = GetConnectionType(atom);

            return Tuple.Create(directions, directions, SectionType.Pipe, connectionType);
        }

        #endregion

        #region Methods

        private static Direction GetAllDirections(Direction original, Direction either, Direction or)
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

        private Direction GetDirections(Atom atom)
        {
            var directions = this.GetDirection(atom, (Direction)2);
            return GetAllDirections(directions, Direction.North, Direction.South)
                   | GetAllDirections(directions, Direction.West, Direction.East);
        }

        #endregion
    }
}