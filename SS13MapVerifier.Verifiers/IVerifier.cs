using System.Collections.Generic;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Verifiers
{
    public interface IVerifier
    {
        IEnumerable<Log> ValidateMap(IMap map);
    }
}