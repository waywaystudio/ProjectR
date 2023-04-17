using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable UnusedMember.Global

public static class EnumerableExtension
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> list) => list == null || !list.Any();
    public static bool HasElement<T>(this IEnumerable<T> list) => !list.IsNullOrEmpty();
        
    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        foreach (var item in list)
            action(item);
    }

    /// <summary>
    /// ForEach with Index : .ForEach((x, index) => x.Index() == index));
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T, int> action)
    {
        var i = 0;
        foreach (var item in sequence)
        {
            action(item, i);
            i++;
        }
    }
        
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector) => source.MinBy(selector, null);
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
    {
        if (source   == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        comparer ??= Comparer<TKey>.Default;

        using var sourceIterator = source.GetEnumerator();

        if (!sourceIterator.MoveNext())
        {
            return default;
        }

        var min    = sourceIterator.Current;
        var minKey = selector(min);

        while (sourceIterator.MoveNext())
        {
            var candidate          = sourceIterator.Current;
            var candidateProjected = selector(candidate);

            if (comparer.Compare(candidateProjected, minKey) < 0)
            {
                min    = candidate;
                minKey = candidateProjected;
            }
        }

        return min;
    }

    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        where TKey : IComparable
    {
        return source.Aggregate((record, next) =>
            Comparer<TKey>.Default.Compare(selector(next), selector(record)) > 0
                ? next
                : record);
    }

    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
        {
            throw new ArgumentNullException(nameof(enumerable));
        }

        // note: creating a Random instance each call may not be correct for you,
        // consider a thread-safe static instance
        var r    = new Random();
        var list = enumerable as IList<T> ?? enumerable.ToList();
        return list.Count == 0 ? default : list[r.Next(0, list.Count)];
    }
}