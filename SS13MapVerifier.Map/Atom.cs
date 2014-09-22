using System;
using System.Collections.Generic;
using System.Linq;

namespace SS13MapVerifier.Map
{
    public class Atom
    {
        #region Constructors and Destructors

        public Atom(string type, IDictionary<string, string> options)
        {
            this.Type = type;
            this.Settings = options;
        }

        #endregion

        #region Public Properties

        public IDictionary<string, string> Settings { get; private set; }

        public string Type { get; private set; }

        #endregion

        #region Public Methods and Operators

        public string GetSetting(string name, string defaultValue = "")
        {
            string value;
            return this.Settings.TryGetValue(name, out value) ? value : defaultValue;
        }

        public override string ToString()
        {
            var settings = string.Join("; ", this.Settings.Select(setting => string.Format("{0} = {1}", setting.Key, setting.Value)));
            return string.Format("{0}{1}{2}{3}", Type.Substring(0, Type.Length - 1), settings.Any() ? "{" : string.Empty, settings, settings.Any() ? "}" : string.Empty);
        }

        #endregion
    }
}
