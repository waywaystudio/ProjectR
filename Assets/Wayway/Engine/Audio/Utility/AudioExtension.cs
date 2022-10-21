using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wayway.Engine.Audio
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
    }
}