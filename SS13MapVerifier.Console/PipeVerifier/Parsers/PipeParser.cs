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

        public override bool CanParse(string content)
        {
            return this.canParse.IsMatch(content);
        }

        public override Tuple<Direction, Direction, SectionType, ContentType> Parse(string content)
        {
            var directions = this.GetDirections(content);
            var connectionType = GetConnectionType(content);

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

        private Direction GetDirections(string content)
        {
            var directions = this.GetDirection(content, (Direction)2);
            return GetAllDirections(directions, Direction.North, Direction.South)
                   | GetAllDirections(directions, Direction.West, Direction.East);
        }

        #endregion
    }
}