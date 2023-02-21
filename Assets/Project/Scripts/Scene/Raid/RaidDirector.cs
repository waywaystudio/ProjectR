using System.Collections.Generic;
using Character;
using Core;
using MainGame;
using UnityEngine;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Raid
{
    public class RaidDirector : MonoBehaviour
    {
        [SerializeField] private RaidCastingDirector castingDirector;
        [SerializeField] private RaidUIDirector uiDirector;
        
        // TODO. TEMP;
        [SerializeField] private AdventurerBehaviour player;

        private AdventurerBehaviour focusCharacter;
        
        public List<AdventurerBehaviour> AdventurerList => castingDirector.AdventurerList;
        public List<MonsterBehaviour> MonsterList => castingDirector.MonsterList;


        private void Awake()
        {
            castingDirector ??= GetComponentInChildren<RaidCastingDirector>();
            uiDirector      ??= GetComponentInChildren<RaidUIDirector>();

            MainUI.FadePanel.PlayFadeIn();

            if (player.IsNullOrEmpty())
            {
                Debug.LogWarning("Required Player");
                return;
            }
        }

        private void Start()
        {
            uiDirector.Initialize(AdventurerList, MonsterList);
        }
    }
}
