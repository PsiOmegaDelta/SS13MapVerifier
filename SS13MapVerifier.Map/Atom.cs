using System.Collections.Generic;
using System.Linq;

namespace SS13MapVerifier.Map
{
    public class Atom
    {
        #region Constructors and Destructors

        public Atom(string type, ILookup<string, string> options)
        {
            this.Type = type;
            this.Settings = options;
        }

        #endregion

        #region Public Properties

        public ILookup<string, string> Settings { get; private set; }

        public string Type { get; private set; }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<string> GetSettings(string name)
        {
            return this.Settings.Contains(name) ? this.Settings[name] : Enumerable.Empty<string>();
        }

        public string GetSetting(string name)
        {
            return this.Settings.Contains(name) ? this.Settings[name].First() : string.Empty;
        }

        #endregion
    }
}
