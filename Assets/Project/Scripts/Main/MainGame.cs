using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine;
using Wayway.Engine.Audio;
using Wayway.Engine.Singleton;

namespace Main
{
    public class MainGame : MonoSingleton<MainGame>
    {
        [SerializeField] private AudioManager audioManager;

        public static AudioManager AudioManager => Instance.audioManager ??= Instance.GetComponentInChildren<AudioManager>();

        [Button]
        public void Function()
        {
            Debug.Log("Function On");
        }
    }
}
