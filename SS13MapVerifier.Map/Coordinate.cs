using System;
using System.Diagnostics;

namespace SS13MapVerifier.Map
{
    [DebuggerDisplay("{X} - {Y} - {Z}")]
    public class Coordinate : IEquatable<Coordinate>
    {
        private readonly int x;
        private readonly int y;
        private readonly int z;

        public Coordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public int Z
        {
            get { return z; }
        }

        public override int GetHashCode()
        {
            // Lazy way of greatly decreasing hash code collision.
            // Using powers of two (rather than 10 & 100) to hopefully get further compile time optimization since it allows for shifting.
            // The naïve x ^ y ^ z (with or without GetHashCode) is more than 5 magnitudes slower when parsing the BS12 map.
            return x ^ (y * 16) ^ (z * 128);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var otherCoord = obj as Coordinate;
            return otherCoord != null && Equals(otherCoord);
        }

        public bool Equals(Coordinate other)
        {
            return x == other.X && y == other.Y && z == other.Z;
        }

        public override string ToString()
        {
            return x + "-" + y + "-" + z;
        }
    }
}
