using MainGame.Manager.Audio.Utility;
using UnityEngine;

namespace MainGame.Manager.Audio
{
    public class AudioClipData : ScriptableObject, IAudioPlayable
    {
        [SerializeField] private AudioType type;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private int priority;

        public AudioType Type => type;
        public AudioClip AudioClip => audioClip;
        public int Priority => priority;

        public void Play() =>  MainManager.Audio.Play(this);
        public void Pause() => MainManager.Audio.Pause(this);
        public void Stop() =>  MainManager.Audio.Stop(this);

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


