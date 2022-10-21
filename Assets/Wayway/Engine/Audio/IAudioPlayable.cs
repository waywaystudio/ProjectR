using UnityEngine;

namespace Wayway.Engine.Audio
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

