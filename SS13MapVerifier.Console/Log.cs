using System.Collections.Generic;
using SS13MapVerifier.Map;

namespace SS13MapVerifier.Console
{
    public enum Severity
    {
        Information, 
        Warning, 
        Error
    }

    public interface ILog
    {
        #region Public Properties

        string Message { get; }

        Severity Severity { get; }

        IEnumerable<ITile> Tiles { get; }

        #endregion
    }

    public class Log : ILog
    {
        #region Fields

        private readonly IList<ITile> tileses = new List<ITile>();

        #endregion

        #region Constructors and Destructors

        public Log(string message, Severity severity)
        {
            this.Severity = severity;
            this.Message = message;
        }

        #endregion

        #region Public Properties

        public string Message { get; private set; }

        public Severity Severity { get; private set; }

        public IEnumerable<ITile> Tiles
        {
            get
            {
                return this.tileses;
            }
        }

        #endregion

        #region Methods

        internal void AddTile(ITile tile)
        {
            this.tileses.Add(tile);
        }

        #endregion
    }
}