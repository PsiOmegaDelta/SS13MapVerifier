using System;
using System.Collections.Generic;

namespace Common
{
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> equalityComparer;

        private readonly Func<T, int> getHasCode;

        public EqualityComparer(Func<T, T, bool> equalityComparer, Func<T, int> getHasCode)
        {
            this.equalityComparer = equalityComparer;
            this.getHasCode = getHasCode;
        }

        public bool Equals(T x, T y)
        {
            return equalityComparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return getHasCode(obj);
        }
    }
}
