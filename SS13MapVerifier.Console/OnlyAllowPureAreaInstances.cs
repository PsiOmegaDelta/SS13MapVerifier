using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common.Extensions;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console
{
    internal class OnlyAllowPureAreaInstances
    {
        #region Fields

        private readonly Regex badContent = new Regex("^/area/.*{(.*)}$");

        #endregion

        #region Public Methods and Operators

        public IEnumerable<Log> ValidateMap(IMap map)
        {
            var errors = new Dictionary<string, Log>();

            foreach (var tile in map.Tiles)
            {
                foreach (var content in tile.Contents)
                {
                    var reg = this.badContent.Match(content);
                    if (reg.Success)
                    {
                        var error = errors.SafeGetValue(
                            reg.Groups[1].Value, 
                            () => new Log("Impure area - " + reg.Groups[1].Value, Severity.Error));
                        error.AddTile(tile);
                    }
                }
            }

            return errors.Values;
        }

        #endregion
    }
}