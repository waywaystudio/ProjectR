using Character;
using UI.ImageUtility;
using UnityEngine;

namespace Raid.UI.ActionFrames.ActionBars.CharacterSkillBars
{
    public class CharacterDashActionSlot : MonoBehaviour
    {
        [SerializeField] private CharacterDashActionIcon skillAction;
        [SerializeField] private ImageFiller coolDownFiller;
        [SerializeField] private BindingCode bindingCode;
        
        private Adventurer focusedAdventurer;

        public void Initialize(Adventurer adventurer) => OnFocusChanged(adventurer);
        public void OnFocusChanged(Adventurer ab)
        {
            // MainGame.MainManager.Input.TryGetAction(bindingCode, out var inputAction);
            // //
            // inputAction.started  -= skillAction.StartAction;
            // inputAction.canceled -= skillAction.CompleteAction;
            //
            // if (ab.IsNullOrEmpty()) return;
            //
            // focusedAdventurer = ab;
            //
            // var actionBehaviour = focusedAdventurer.ActionBehaviour;
            // var dashSkill = actionBehaviour.DashSkill;
            //
            // skillAction.Initialize(ab, dashSkill);
            // coolDownFiller.Register(dashSkill.RemainCoolTime, dashSkill.CoolTime);
            //
            // inputAction.started  += skillAction.StartAction;
            // inputAction.canceled += skillAction.CompleteAction;
        }
    }
}
