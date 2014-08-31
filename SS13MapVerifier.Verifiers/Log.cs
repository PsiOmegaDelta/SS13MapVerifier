using System.Collections.Generic;

using SS13MapVerifier.Map;

namespace SS13MapVerifier.Verifiers
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

        private readonly IList<ITile> tiles = new List<ITile>();

        #endregion

        #region Constructors and Destructors

        public Log(string message, Severity severity)
        {
            this.Severity = severity;
            this.Message = message;
        }

        public Log(string message, Severity severity, ITile tile)
        {
            this.Severity = severity;
            this.Message = message;
            this.tiles.Add(tile);
        }

        public Log(string message, Severity severity, IEnumerable<ITile> tiles)
        {
            this.Severity = severity;
            this.Message = message;
            foreach (var tile in tiles)
            {
                this.tiles.Add(tile);
            }
        }

        #endregion

        #region Public Properties

        public string Message { get; private set; }

        public Severity Severity { get; private set; }

        public IEnumerable<ITile> Tiles
        {
            get
            {
                return this.tiles;
            }
        }

        #endregion

        #region Methods

        internal void AddTile(ITile tile)
        {
            this.tiles.Add(tile);
        }

        #endregion
    }
}