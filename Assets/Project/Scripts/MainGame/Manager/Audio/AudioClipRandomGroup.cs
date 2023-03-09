using System.Collections.Generic;
using Manager.Audio.Utility;
using UnityEngine;

namespace Manager.Audio
{
    public class AudioClipRandomGroup : ScriptableObject, IAudioPlayable
    {
        [SerializeField] private List<AudioClipData> clipList;
        private AudioClipData pickedClip;

        public AudioType Type => pickedClip != null ? pickedClip.Type : AudioType.Bumper;
        public AudioClip AudioClip => pickedClip != null ? pickedClip.AudioClip : null;
        public int Priority => pickedClip != null ? pickedClip.Priority : 256;

        public void Play()
        {
            if (clipList == null) return;

            pickedClip = clipList.Random();
            pickedClip.Play();
        }

        public void Pause() { if (pickedClip != null) pickedClip.Pause(); }
        public void Stop() { if (pickedClip != null) pickedClip.Stop(); }

#if UNITY_EDITOR
        #region editor Function :: play random & stop
        private void PlayPreviewClip()
        {
            AudioClipUtility.StopAllClips();
            AudioClipUtility.PlayClip(clipList.Random().AudioClip);
        }
        private void StopPreviewClip() => AudioClipUtility.StopAllClips();
        #endregion
#endif
    }
}


