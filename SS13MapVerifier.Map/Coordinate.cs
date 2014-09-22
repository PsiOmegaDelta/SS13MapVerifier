using System;
using System.Diagnostics;

namespace SS13MapVerifier.Map
{
    [DebuggerDisplay("{X} - {Y} - {Z}")]
    public class Coordinate : IEquatable<Coordinate>
    {
        #region Fields

        private readonly int x;
        private readonly int y;
        private readonly int z;

        #endregion

        #region Constructors and Destructors

        public Coordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region Public Properties

        public int X
        {
            get
            {
                return this.x;
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }
        }

        public int Z
        {
            get
            {
                return this.z;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            var otherCoord = obj as Coordinate;
            return otherCoord != null && this.Equals(otherCoord);
        }

        public bool Equals(Coordinate other)
        {
            return ReferenceEquals(this, other) || (this.x == other.X && this.y == other.Y && this.z == other.Z);
        }

        public override int GetHashCode()
        {
            // Lazy way of greatly decreasing hash code collision.
            // Using powers of two (rather than 10 & 100) to hopefully get further compile time optimization since it allows for shifting.
            // The naïve x ^ y ^ z (with or without GetHashCode) is more than 5 magnitudes slower when parsing the BS12 map.
            return this.x ^ (this.y * 16) ^ (this.z * 128);
        }

        public override string ToString()
        {
            return this.x + "-" + this.y + "-" + this.z;
        }

        #endregion
    }
}