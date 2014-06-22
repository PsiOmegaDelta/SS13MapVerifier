using System;
using System.Text.RegularExpressions;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier.Parsers
{
    internal abstract class SectionParser
    {
        #region Fields

        private readonly Regex getDir = new Regex("dir = (\\d+)");

        #endregion

        #region Public Methods and Operators

        public abstract bool CanParse(string content);

        public abstract Tuple<Direction, Direction, SectionType, ContentType> Parse(string content);

        #endregion

        #region Methods

        protected static ContentType GetConnectionType(string content)
        {
            if (content.Contains("cyan"))
            {
                return ContentType.Cyan;
            }

            if (content.Contains("green"))
            {
                return ContentType.Green;
            }

            if (content.Contains("scrubbers"))
            {
                return ContentType.Scrubbers;
            }

            if (content.Contains("supply"))
            {
                return ContentType.Supply;
            }

            if (content.Contains("yellow"))
            {
                return ContentType.Yellow;
            }

            return ContentType.Any;
        }

        protected Direction GetDirection(string content, Direction defaultDirection)
        {
            var regDir = this.getDir.Match(content);
            return regDir.Success ? (Direction)int.Parse(regDir.Groups[1].Value) : defaultDirection;
        }

        #endregion
    }
}