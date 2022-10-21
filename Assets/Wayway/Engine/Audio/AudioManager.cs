using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Wayway.Engine.Singleton;

namespace Wayway.Engine.Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private List<AudioChannel> audioChannels;

        private Dictionary<AudioType, AudioChannel> audioTable = new ();
        private AudioSource oneShotSource;

        /* Exposed Volume Parameter from AudioMixer >> Volume.EVP  */
        private const string MasterEvp = "MasterVolume";
        private const string BgmEvp = "BgmVolume";
        private const string SfxEvp = "SfxVolume";
        private const string BumperEvp = "BumperVolume";

        public static Dictionary<AudioType, AudioChannel> AudioTable => Instance.audioTable;
        public static AudioSource AudioSource => Instance.oneShotSource;

        public AudioSource OneShotSource => oneShotSource;

        public void SetUp()
        {
            oneShotSource = GetComponent<AudioSource>();

            GetComponentsInChildren(audioChannels);

            audioChannels.ForEach(x => x.SetUp());
            audioChannels.ForEach(x => audioTable.Add(x.Type, x));
            audioTable.ForEach(x => x.Value.ExposedVolumeParameter = GetEvp(x.Value.Type));
        }

        public static void Play(IAudioPlayable clipData) { if (TryGetChannel(clipData, out var source)) source.Play(clipData); }
        public static void Pause(IAudioPlayable clipData) { if (TryGetChannel(clipData, out var source)) source.Pause(clipData); }
        public static void Stop(IAudioPlayable clipData) { if (TryGetChannel(clipData, out var source)) source.Stop(clipData); }

        public void StopAll() => audioChannels.ForEach(x => x.StopAll());
        public void StopAll(AudioType type) { if (TryGetChannel(type, out var result)) result.StopAll(); }

        public void PlayOneShot(AudioClipData audioClipData) { if (OneShotSource != null) OneShotSource.PlayOneShot(audioClipData.AudioClip); }
        public void StopOneShot() { if (OneShotSource != null) OneShotSource.Stop(); }

        // Volume Control
        public void MasterFadeOut(float duration) => MasterFadeOut(duration, () => { StopAll(); MasterFadeIn(0); });
        public void MasterFadeOut(float duration, Action callback) => MasterVolumeFade(-80f, duration, callback);
        public void MasterFadeIn(float duration) => MasterVolumeFade(0f, duration);
        public void AudioFadeOut(AudioType type, float duration) { if (TryGetChannel(type, out var result)) result.AudioFadeOut(duration); }
        public void AudioFadeIn(AudioType type, float duration) { if (TryGetChannel(type, out var result)) result.AudioFadeIn(duration); }
        public void Mute(AudioType type, bool value) { if (TryGetChannel(type, out var result)) result.Mute(value); }
        public void MuteAll(bool value) => audioTable.ForEach((pair) => Mute(pair.Key, value));


        private static bool TryGetChannel(IAudioPlayable clipData, out AudioChannel result) => AudioTable.TryGetValue(clipData.Type, out result);
        private static bool TryGetChannel(AudioType type, out AudioChannel result) => AudioTable.TryGetValue(type, out result);
        private void SetMasterVolume(float destVolume) => masterMixer.SetFloat("MasterVolume", destVolume);
        private void MasterVolumeFade(float target, float duration) => MasterVolumeFade(target, duration, null);
        private void MasterVolumeFade(float target, float duration, Action callback)
        {
            masterMixer.GetFloat(MasterEvp, out var currentVolume);

            // LeanTween.cancel(transform.gameObject);
            // LeanTween.value(transform.gameObject, SetMasterVolume, currentVolume, target, duration).setOnComplete(callback);
        }

        private string GetEvp(AudioType type) => type switch
        {
            AudioType.Bgm => BgmEvp,
            AudioType.Sfx => SfxEvp,
            _ => BumperEvp,
        };

#if UNITY_EDITOR
        #region editor Function :: GetAudioChannels
        private void GetAudioChannels()
        {
            GetComponentsInChildren(audioChannels);
            audioChannels.ForEach(x => audioTable.Add(x.Type, x));
        }
        #endregion
#endif
    }
}

