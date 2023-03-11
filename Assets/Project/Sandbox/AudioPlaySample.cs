using System.Collections;
using System.Collections.Generic;
using Manager;
using Manager.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioPlaySample : MonoBehaviour
{
    public AudioClipData Clip;

    public void Test() => Clip.Play();
    
    [Button]
    public void Test2() => MainManager.Audio.Play(Clip);

    [Button]
    public void Text3() => AudioManager.Play(Clip);
}

public static class AudioManager
{
    public static void Play(AudioClipData clipData) => MainManager.Audio.Play(clipData);
}
