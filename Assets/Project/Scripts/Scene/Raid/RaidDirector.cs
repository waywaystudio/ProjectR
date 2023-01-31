using System.Collections.Generic;
using Character;
using Core;
using Core.GameEvents;
using MainGame;
using UnityEngine;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Raid
{
    public class RaidDirector : MonoBehaviour
    {
        public GameEventTransform OnFocusCharacterChanged;

        [SerializeField] private RaidCastingDirector castingDirector;
        [SerializeField] private RaidUIDirector uiDirector;
        
        // TODO. TEMP;
        [SerializeField] private AdventurerBehaviour player;

        private Transform focusCharacter;
        
        public List<AdventurerBehaviour> AdventurerList => castingDirector.AdventurerList;
        public List<MonsterBehaviour> MonsterList => castingDirector.MonsterList;

        public Transform FocusCharacter
        {
            get => focusCharacter;
            set
            {
                if (value == focusCharacter) return;
                
                focusCharacter             = value;
                uiDirector.FocusAdventurer = FocusCharacter.GetComponent<AdventurerBehaviour>();
                OnFocusCharacterChanged.Invoke(value);
            }
        }
        

        private void Awake()
        {
            castingDirector ??= GetComponentInChildren<RaidCastingDirector>();
            uiDirector      ??= GetComponentInChildren<RaidUIDirector>();
            
            uiDirector.Initialize(AdventurerList, MonsterList);
            
            MainUI.FadePanel.PlayFadeIn();

            if (player.IsNullOrEmpty())
            {
                Debug.LogWarning("Required Player");
                return;
            }

            FocusCharacter = player.transform;
        }
    }
}
