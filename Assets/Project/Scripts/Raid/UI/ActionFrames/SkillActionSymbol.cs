using Character.Adventurers;
using Common;
using Common.Characters.Behaviours;
using Common.Skills;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.ActionFrames
{
    public class SkillActionSymbol : MonoBehaviour, ITooltipInfo
    {
        [SerializeField] protected Image actionImage;
        
        private SkillComponent skillComponent;
        private Adventurer focusedAdventurer;
        private Transform preTransform;
        private SkillBehaviour characterAction => focusedAdventurer.SkillBehaviour;


        public void Initialize(Adventurer adventurer, SkillComponent skillComponent)
        {
            focusedAdventurer = adventurer; 

            this.skillComponent = skillComponent;
            actionImage.sprite  = skillComponent.Icon;
        }

        public void StartAction(InputAction.CallbackContext context)
        {
            if (skillComponent.IsNullOrEmpty()) return;
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;

            characterAction.Active(skillComponent, mousePosition);
        }

        public void ReleaseAction(InputAction.CallbackContext context)
        {
            if (skillComponent.IsNullOrEmpty() || focusedAdventurer.IsNullOrEmpty()) return;

            characterAction.Release();
        }

        public string TooltipInfo { get; }
    }
}
