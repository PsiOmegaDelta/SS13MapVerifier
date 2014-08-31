using System;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Verifiers.PipeVerifier.Parsers
{
    internal class ManifoldParser : SectionParser
    {
        private const string CanParseManifold = "/obj/machinery/atmospherics/pipe/manifold";
        private const string CanParseManifold4W = "/obj/machinery/atmospherics/pipe/manifold4w";

        public override bool CanParse(Atom atom)
        {
            return atom.Type.StartsWith(CanParseManifold) || atom.Type.StartsWith(CanParseManifold4W);
        }

        public override Tuple<Directions, Directions, SectionType, ContentType> Parse(Atom atom)
        {
            var isFourway = atom.Type.StartsWith(CanParseManifold4W);
            var direction = isFourway ? Directions.Cardinal : AcquireRelevantDirections(atom);
            var contentType = GetContentType(atom);

            return Tuple.Create(
                direction,
                direction,
                isFourway ? SectionType.ManifoldFourway : SectionType.Manifold,
                contentType);
        }
        
        private static Directions AcquireRelevantDirections(Atom  atom)
        {
            int dir;
            var enumDirection = int.TryParse(atom.GetSetting("dir"), out dir) ? (Directions)dir : (Directions)2;
            enumDirection = GetManifoldDirections(enumDirection);
            return enumDirection;
        }


        private static Directions GetManifoldDirections(Directions enumDirection)
        {
            return Directions.Cardinal ^ enumDirection;
        }
    }
}
