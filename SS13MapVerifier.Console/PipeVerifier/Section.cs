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
        Pump
    }

    [DebuggerDisplay("{Directions} - {ContentType} - {SectionType}")]
    public class Section
    {
        #region Static Fields

        private static readonly IEnumerable<SectionParser> Parsers = new List<SectionParser> { new PipeParser(), new ManifoldParser() };

        #endregion

        #region Constructors and Destructors

        public Section(ITile tile, string content)
        {
            this.Tile = tile;
            this.Content = content;
            var parser = Parsers.Single(x => x.CanParse(content));
            var result = parser.Parse(content);
            this.Input = result.Item1;
            this.Output = result.Item2;
            this.SectionType = result.Item3;
            this.ContentType = result.Item4;
        }

        #endregion

        #region Public Properties

        public string Content { get; private set; }

        public ContentType ContentType { get; private set; }

        public Direction Directions
        {
            get
            {
                return this.Input | this.Output;
            }
        }

        public bool FullyConnected { get; set; }

        public bool HasBeenVisited { get; set; }

        public Direction Input { get; private set; }

        public Direction Output { get; private set; }

        public SectionType SectionType { get; private set; }

        public ITile Tile { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static bool IsSection(string content)
        {
            return Parsers.SingleOrDefault(x => x.CanParse(content)) != null;
        }

        #endregion
    }
}