using System;

using SS13MapVerifier.Map;
using SS13MapVerifier.Map.Constants;

namespace SS13MapVerifier.Verifiers.PipeVerifier.Parsers
{
    internal class OmniParser : SectionParser
    {
        private const string CanParseType = Objects.Omni;

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseType);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var input = Directions.None;
            var output = Directions.None;
            foreach (var direction in new[]
                                          {
                                              new { Direction = Directions.North, Setting = "tag_north"},
                                              new { Direction = Directions.East, Setting = "tag_east"},
                                              new { Direction = Directions.South, Setting = "tag_south"},
                                              new { Direction = Directions.West, Setting = "tag_west"}
                                          })
            {
                var gasSetting = atom.GetSetting(direction.Setting, "0");
                if (gasSetting == "1")
                {
                    input |= direction.Direction;
                }
                else if (gasSetting != "0")
                {
                    output |= direction.Direction;
                }
            }

            return Tuple.Create(input, output, SectionType.Omni, ContentType.Any);
        }
    }
}
