using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace SS13MapVerifier.Map
{
    public class Atom
    {
        // There is a way to capture everything within the first { and last } without including them (while considering any additional } & { inside)
        // by utilizing negative/positive lookahead or w.e. but someone else can hack that regex-expression if they want

        #region Fields

        private readonly Regex matcher = new Regex("^([^{]*)({.*})?$");

        private readonly ReadOnlyDictionary<string, string> settings;

        #endregion

        #region Constructors and Destructors

        public Atom(string content)
        {
            var regResult = this.matcher.Match(content);
            if (!regResult.Success)
            {
                throw new InvalidDataException("content");
            }

            this.Type = regResult.Groups[1].Value;
            var values = regResult.Groups[2].Value;
            this.settings = new ReadOnlyDictionary<string, string>(GetDictionaryFromContent(values));
        }

        #endregion

        #region Public Properties

        public ReadOnlyDictionary<string, string> Settings
        {
            get
            {
                return this.settings;
            }
        }

        public string Type { get; private set; }

        #endregion

        #region Public Methods and Operators

        public string Setting(string name)
        {
            string result;
            return this.Settings.TryGetValue(name, out result) ? result : string.Empty;
        }

        #endregion

        #region Methods

        // This section is just horrible, just barely a hack that works
        private static IEnumerable<Tuple<string, string>> GetSettingsFromContent(string content)
        {
            var assignmentIndex = content.IndexOf('=');
            var key = content.Substring(0, assignmentIndex++);
            string value;

            var quoteIndex = content.IndexOf('"', assignmentIndex);
            var semiColinIndex = content.IndexOf(';', assignmentIndex);

            if (semiColinIndex == -1)
            {
                value = content.Substring(assignmentIndex);
            }
            else if (quoteIndex != -1 && quoteIndex < semiColinIndex)
            {
                var secondQuoteIndex = content.IndexOf("\";", StringComparison.Ordinal);
                value = content.Substring(quoteIndex, (secondQuoteIndex) - quoteIndex);
                semiColinIndex = secondQuoteIndex + 1;
            }
            else
            {
                value = content.Substring(assignmentIndex, semiColinIndex - assignmentIndex);
            }

            yield return Tuple.Create(key.Trim(), value.Trim(new[] { ' ', '"' }));
            if (semiColinIndex > -1)
            {
                var subContent = content.Substring(semiColinIndex + 1);
                foreach (var setting in GetSettingsFromContent(subContent))
                {
                    yield return setting;
                }
            }

        }

        private static IDictionary<string, string> GetDictionaryFromContent(string value)
        {
            var dictionary = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(value))
            {
                return dictionary;
            }

            value = value.Substring(1, value.Length - 2);
            foreach (var tuple in GetSettingsFromContent(value))
            {
                dictionary.Add(tuple.Item1, tuple.Item2);
            }

            return dictionary;
        }

        #endregion
    }
}
