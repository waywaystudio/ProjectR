using UnityEngine;
using AudioType = Main.Audio.AudioType;

namespace Main.Audio
{
    public class AudioClipData : ScriptableObject, IAudioPlayable
    {
        [SerializeField] private AudioType type;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private int priority;

        public AudioType Type => type;
        public AudioClip AudioClip => audioClip;
        public int Priority => priority;

        public void Play() => MainGame.AudioManager.Play(this);
        public void Pause() => MainGame.AudioManager.Pause(this);
        public void Stop() => MainGame.AudioManager.Stop(this);

#if UNITY_EDITOR
        #region editor Function :: play & stop
        private void PlayPreviewClip()
        {
            AudioClipUtility.StopAllClips();
            AudioClipUtility.PlayClip(AudioClip);
        }
        private void StopPreviewClip() => AudioClipUtility.StopAllClips();
        #endregion
#endif
    }
}


