using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

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

        private static readonly IEnumerable<ISectionParser> Parsers = new List<ISectionParser> { new PipeParser() };

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

        #region Interfaces

        internal interface ISectionParser
        {
            #region Public Methods and Operators

            bool CanParse(string content);

            Tuple<Direction, Direction, SectionType, ContentType> Parse(string content);

            #endregion
        }

        #endregion

        #region Public Properties

        public ContentType ContentType { get; private set; }

        public ITile Tile { get; private set; }

        public string Content { get; private set; }

        public Direction Directions
        {
            get
            {
                return Input | Output;
            }
        }

        public Direction Input { get; private set; }

        public Direction Output { get; private set; }

        public bool HasBeenVisited { get; set; }

        public bool FullyConnected { get; set; }

        public SectionType SectionType { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static bool IsSection(string content)
        {
            return Parsers.SingleOrDefault(x => x.CanParse(content)) != null;
        }

        #endregion

        private class PipeParser : ISectionParser
        {
            #region Fields

            private readonly Regex canParse = new Regex("^/obj/machinery/atmospherics/pipe/simple.*$");

            private readonly Regex getDir = new Regex("dir = (\\d+)");

            #endregion

            #region Public Methods and Operators

            public bool CanParse(string content)
            {
                return this.canParse.IsMatch(content);
            }

            public Tuple<Direction, Direction, SectionType, ContentType> Parse(string content)
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

            private static ContentType GetConnectionType(string content)
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

            private Direction GetDirections(string content)
            {
                var regedDir = this.getDir.Match(content);
                var directions = regedDir.Success ? (Direction)int.Parse(regedDir.Groups[1].Value) : (Direction)2;
                return GetAllDirections(directions, Direction.North, Direction.South)
                       | GetAllDirections(directions, Direction.West, Direction.East);
            }

            #endregion
        }
    }
}
