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

    public class Log : ILog
    {
        private readonly IList<ITile> tileses = new List<ITile>();

        public Log(string message, Severity severity)
        {
            this.Severity = severity;
            this.Message = message;
        }

        public IEnumerable<ITile> Tiles
        {
            get
            {
                return this.tileses;
            }
        }

        public string Message { get; private set; }

        public Severity Severity { get; private set; }

        internal void AddTile(ITile tile)
        {
            this.tileses.Add(tile);
        }
    }

    public interface ILog
    {
        IEnumerable<ITile> Tiles { get; }

        string Message { get; }

        Severity Severity { get; }
    }
}
