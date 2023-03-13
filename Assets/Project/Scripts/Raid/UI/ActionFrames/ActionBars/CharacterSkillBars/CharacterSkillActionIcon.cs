using Character;
using Character.Actions;
using Character.Adventurers;
using Common;
using Common.Actions;
using Common.Skills;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.ActionFrames.ActionBars.CharacterSkillBars
{
    public class CharacterSkillActionIcon : MonoBehaviour, ITooltipInfo
    {
        [SerializeField] private Image actionImage;

        private SkillComponent skillComponent;
        private Adventurer focusedAdventurer;
        private Transform preTransform;
        private ActionBehaviour characterAction => focusedAdventurer.CharacterAction;
        

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

            characterAction.ActiveSkill(skillComponent, mousePosition);
        }

        public void ReleaseAction(InputAction.CallbackContext context)
        {
            if (skillComponent.IsNullOrEmpty() || focusedAdventurer.IsNullOrEmpty()) return;
            if (skillComponent.SkillType is SkillType.Instant or SkillType.Casting) return;
            
            characterAction.ReleaseSkill();
        }

        public string TooltipInfo => actionImage ? actionImage.sprite.ToString() : "None";
    }
}
