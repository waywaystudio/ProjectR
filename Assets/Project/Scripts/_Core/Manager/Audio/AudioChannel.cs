using System;
using System.Collections.Generic;
using System.Linq;
using Manager.Audio.Utility;
using UnityEngine;
using UnityEngine.Audio;

namespace Manager.Audio
{
    public class AudioChannel : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup audioMixerGroup;
        [SerializeField] private AudioType type;
        [SerializeField] private List<AudioSource> audioSourceList;

        private AudioMixer master;
        private string exposedVolumeParameter = "";

        public AudioType Type => type;
        public string ExposedVolumeParameter { set => exposedVolumeParameter = value; }
        public List<AudioSource> AudioSourceList => audioSourceList ??= GetComponentsInChildren<AudioSource>().ToList();

        public void SetUp()
        {
            AudioSourceList.ForEach(x =>
            {
                x.outputAudioMixerGroup = audioMixerGroup;
                x.priority = 256;

                if (Type == AudioType.Bgm)
                {
                    x.loop = true;
                }
            });

            master = audioMixerGroup.audioMixer;
        }

        public void Play(IAudioPlayable clipData)
        {
            if (TryGetFreeSource(out var source))
            {
                source.Play(clipData.AudioClip);                
                return;
            }

            source = GetLessPrioritySource();

            if (clipData.Priority >= source.priority) return;
            
            if (source.isPlaying) 
                source.Stop();

            source.Play(clipData.AudioClip);
        }

        public void Pause(IAudioPlayable clipData) { if (TryGetSource(clipData, out var source)) source.Pause(); }
        public void Stop(IAudioPlayable clipData) { if (TryGetSource(clipData, out var source)) source.Stop(); }
        public void StopAll() => audioSourceList.ForEach(x => x.Stop());
        public void Mute(bool value) => audioSourceList.ForEach(x => x.mute = value);
        public void AudioFadeIn(float duration) => SetVolume(-0f, duration);
        public void AudioFadeOut(float duration) => SetVolume(-80f, duration, () => { StopAll(); AudioFadeIn(0); });


        private bool TryGetFreeSource(out AudioSource result)
        {
            result = audioSourceList.Find(x => !x.isPlaying);

            return result != null;
        }

        private bool TryGetSource(IAudioPlayable clipData, out AudioSource result)
        {
            result = audioSourceList.Find(x => x.clip == clipData.AudioClip);

            return result != null;
        }

        private AudioSource GetLessPrioritySource() => audioSourceList.MinBy(x => x.priority);

        private void SetMixerGroupVolume(float destVolume) => master.SetFloat(exposedVolumeParameter, destVolume);
        private void SetVolume(float targetVolume, float duration) => SetVolume(targetVolume, duration, null);
        private void SetVolume(float targetVolume, float duration, Action callback)
        {
            if (duration <= 0) SetMixerGroupVolume(targetVolume);

            master.GetFloat(exposedVolumeParameter, out var currentVolume);

            // LeanTween.cancel(transform.gameObject);
            // LeanTween.value(transform.gameObject, SetMixerGroupVolume, currentVolume, targetVolume, duration)
            //          .setIgnoreTimeScale(true)
            //          .setOnComplete(callback);
        }

#if UNITY_EDITOR && ODIN_INSPECTOR
        #region - editor Function :: Initialize
        [Sirenix.OdinInspector.Button(Sirenix.OdinInspector.ButtonSizes.Large)]
        private void GetAudioSourcesInChildren()
        {
            audioSourceList = GetComponentsInChildren<AudioSource>().ToList();
            audioSourceList.ForEach(x =>
            {
                x.outputAudioMixerGroup = audioMixerGroup;
                x.priority = 256;

                if (Type == AudioType.Bgm)
                {
                    x.loop = true;
                }
            });

            master = audioMixerGroup.audioMixer;
        }
        #endregion
#endif
    }
}


