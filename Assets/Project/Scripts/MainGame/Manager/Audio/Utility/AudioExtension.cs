using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Manager.Audio.Utility
{
    public static class AudioExtension
    {
        public static void Play(this AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public static void Play(this AudioSource audioSource, AudioClipData clipData)
        {
            audioSource.priority = clipData.Priority;
            audioSource.clip = clipData.AudioClip;

            audioSource.Play();
        }
        
        public static TSource MinBy<TSource, TKey>(this List<TSource> list, Func<TSource, TKey> selector) => list.MinBy(selector, null);
        public static TSource MinBy<TSource, TKey>(this List<TSource> list, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            comparer ??= Comparer<TKey>.Default;

            using var sourceIterator = list.GetEnumerator();

            if (!sourceIterator.MoveNext())
            {
                return default;
            }

            var min = sourceIterator.Current;
            var minKey = selector(min);

            while (sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);

                if (comparer.Compare(candidateProjected, minKey) < 0)
                {
                    min = candidate;
                    minKey = candidateProjected;
                }
            }

            return min;
        }
        
        public static T Random<T>(this List<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            // note: creating a Random instance each call may not be correct for you,
            // consider a thread-safe static instance
            var r = new System.Random();            
            return list.Count == 0 ? default : list[r.Next(0, list.Count)];
        }
        
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
                action(item);
        }
    }
}