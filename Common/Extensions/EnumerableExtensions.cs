using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static int CompareTo<T>(this IEnumerable<T> source, IEnumerable<T> target)
            where T : IComparable<T>
        {
            if (ReferenceEquals(source, target))
            {
                return 0;
            }

            var firstEnumerator = source.GetEnumerator();
            var secondEnumerator = target.GetEnumerator();
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

        public static bool Equal<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            if (ReferenceEquals(source, target))
            {
                return true;
            }

            var firstEnumerator = source.GetEnumerator();
            var secondEnumerator = target.GetEnumerator();
            var firstHadNext = firstEnumerator.MoveNext();
            var secondHadNext = secondEnumerator.MoveNext();

            while (firstHadNext && secondHadNext)
            {
                if (!firstEnumerator.Current.Equals(secondEnumerator.Current))
                {
                    return false;
                }

                firstHadNext = firstEnumerator.MoveNext();
                secondHadNext = secondEnumerator.MoveNext();
            }

            if (firstHadNext != secondHadNext)
            {
                return false;
            }

            return true;
        }
    }
}
