﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.PipeVerifier.Parsers
{
    internal class ManifoldParser : SectionParser
    {
        public override bool CanParse(string content)
        {
            throw new NotImplementedException();
        }

        public override Tuple<Direction, Direction, SectionType, ContentType> Parse(string content)
        {
            throw new NotImplementedException();
        }
    }
}