using System.Collections.Generic;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console.Verifiers
{
    internal interface IVerifier
    {
        IEnumerable<Log> ValidateMap(IMap map);
    }
}