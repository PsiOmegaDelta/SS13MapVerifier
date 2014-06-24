using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using SS13MapVerifier.Console.PipeVerifier.Parsers;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier
{
    [Flags]
    public enum ContentType
    {
        None = 0,
        Cyan = 1,
        Green = 2,
        Scrubbers = 4,
        Supply = 8,
        Yellow = 16,
        Any = Cyan | Green | Scrubbers | Supply | Yellow
    }

    public enum SectionType
    {
        Pipe,
        Pump,
        Binary,
        Unary,
        Manifold,
        ManifoldFourway
    }

    [DebuggerDisplay("{SectionType} - {ContentType} - {Directions}")]
    public class Section
    {
        #region Static Fields

        private static readonly IEnumerable<SectionParser> Parsers = new List<SectionParser> { new PipeParser(), new ManifoldParser(), new UnaryParser(), new BinaryParser() };

        #endregion

        #region Constructors and Destructors

        public Section(ITile tile, Atom atom)
        {
            this.Tile = tile;
            var parser = Parsers.Single(x => x.CanParse(atom));
            var result = parser.Parse(atom);
            this.Input = result.Item1;
            this.Output = result.Item2;
            this.SectionType = result.Item3;
            this.ContentType = result.Item4;
        }

        #endregion

        #region Public Properties

        public ContentType ContentType { get; private set; }

        public Directions Directions
        {
            get
            {
                return this.Input | this.Output;
            }
        }

        public Directions Input { get; private set; }

        public Directions Output { get; private set; }

        public SectionType SectionType { get; private set; }

        public ITile Tile { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static bool IsSection(Atom atom)
        {
            return Parsers.SingleOrDefault(x => x.CanParse(atom)) != null;
        }

        public override string ToString()
        {

            return string.Format("{0} - {1} - {2}", SectionType, ContentType, Directions);
        }

        #endregion
    }
}