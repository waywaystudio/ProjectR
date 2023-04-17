using System.Collections.Generic;

public static class CollectionExtension
{
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;
    public static bool HasElement<T>(this ICollection<T> collection) => !collection.IsNullOrEmpty();
    public static void AddUniquely<T>(this ICollection<T> collection, T item)
    {
        if (collection.Contains(item)) return;
        collection.Add(item);
    }

    public static void RemoveSafely<T>(this ICollection<T> collection, T item)
    {
        if (!collection.Contains(item)) return;
        collection.Remove(item);
    }
    
    public static int ElementCount<T>(this ICollection<T> collection) 
        => collection.IsNullOrEmpty() ? 0 : collection.Count;

    public static bool MoreThen<T>(this ICollection<T> collection, int count)
        => collection.HasElement() && collection.Count >= count;
}
