using System;
using System.Collections.Generic;

namespace Common
{
    public class Comparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> comparer;

        public Comparer(Func<T,T,int> comparer)
        {
            this.comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return comparer(x, y);
        }
    }
}
