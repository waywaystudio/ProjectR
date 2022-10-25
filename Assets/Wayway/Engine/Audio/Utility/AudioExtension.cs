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
    }
}