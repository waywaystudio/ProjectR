using UnityEngine;

namespace Main.Audio
{
    public interface IAudioPlayable
    {
        AudioType Type { get; }
        AudioClip AudioClip { get; }
        int Priority { get; }

        void Play();
        void Pause();
        void Stop();
    }
}

