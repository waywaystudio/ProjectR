using System.Collections.Generic;
using Character;
using Core.GameEvents;
using MainGame;
using UnityEngine;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Scene.Raid
{
    public class RaidDirector : MonoBehaviour
    {
        public GameEventTransform OnCharacterFocusChanged;
        
        public List<AdventurerBehaviour> AdventurerList { get; set; } = new();
        public List<MonsterBehaviour> MonsterList { get; set; } = new();

        private void Awake()
        {
            MainUI.FadePanel.PlayFadeIn();
        }
    }
}
