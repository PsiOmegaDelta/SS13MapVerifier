using System;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier.Parsers
{
    internal abstract class SectionParser
    {
        #region Public Methods and Operators

        public abstract bool CanParse(Atom atom);

        public abstract Tuple<Direction, Direction, SectionType, ContentType> Parse(Atom atom);

        #endregion

        #region Methods

        protected static ContentType GetConnectionType(Atom atom)
        {
            if (atom.Type.Contains("cyan"))
            {
                return ContentType.Cyan;
            }

            if (atom.Type.Contains("green"))
            {
                return ContentType.Green;
            }

            if (atom.Type.Contains("scrubbers"))
            {
                return ContentType.Scrubbers;
            }

            if (atom.Type.Contains("supply"))
            {
                return ContentType.Supply;
            }

            if (atom.Type.Contains("yellow"))
            {
                return ContentType.Yellow;
            }

            return ContentType.Any;
        }

        protected Direction GetDirection(Atom atom, Direction defaultDirection)
        {
            int result;
            return int.TryParse(atom.Setting("dir"), out result) ? (Direction)result : defaultDirection;
        }

        #endregion
    }
}