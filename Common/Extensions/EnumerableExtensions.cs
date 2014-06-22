using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static int CompareTo<T>(IEnumerable<T> source, IEnumerable<T> sequence)
            where T : IComparable<T>
        {
            if (ReferenceEquals(source, sequence))
            {
                return 0;
            }

            var firstEnumerator = source.GetEnumerator();
            var secondEnumerator = sequence.GetEnumerator();
            var firstHadNext = firstEnumerator.MoveNext();
            var secondHadNext = secondEnumerator.MoveNext();

            while (firstHadNext && secondHadNext)
            {
                var compare = firstEnumerator.Current.CompareTo(secondEnumerator.Current);
                if (compare != 0)
                {
                    return compare;
                }

                firstHadNext = firstEnumerator.MoveNext();
                secondHadNext = secondEnumerator.MoveNext();
            }

            if (!firstHadNext && !secondHadNext)
            {
                return 0;
            }

            return firstHadNext ? 1 : -1;
        }
    }
}
