using Character;
using Core;
using UnityEngine;

namespace Raid.UI.ActionFrames.ActionBars.CharacterSkillBars
{
    public class CharacterDashActionSlot : MonoBehaviour
    {
        [SerializeField] private CharacterDashActionIcon skillAction;
        [SerializeField] private ImageFiller coolDownFiller;
        [SerializeField] private BindingCode bindingCode;
        
        private AdventurerBehaviour focusedAdventurer;

        public void Initialize(AdventurerBehaviour adventurer) => OnFocusChanged(adventurer);
        public void OnFocusChanged(AdventurerBehaviour ab)
        {
            MainGame.MainManager.Input.TryGetAction(bindingCode, out var inputAction);
            //
            inputAction.started  -= skillAction.StartAction;
            inputAction.canceled -= skillAction.CompleteAction;

            if (ab.IsNullOrEmpty()) return;
            
            focusedAdventurer = ab;
            
            var actionBehaviour = focusedAdventurer.ActionBehaviour;
            var dashSkill = actionBehaviour.DashSkill;

            skillAction.Initialize(ab, dashSkill);
            coolDownFiller.Register(dashSkill.RemainCoolTime, dashSkill.CoolTime);
            
            inputAction.started  += skillAction.StartAction;
            inputAction.canceled += skillAction.CompleteAction;
        }
    }
}
