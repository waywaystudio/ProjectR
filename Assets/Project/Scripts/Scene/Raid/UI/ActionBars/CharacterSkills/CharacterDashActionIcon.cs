using Character;
using Character.Skill;
using Core;
using MainGame;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.ActionBars.CharacterSkills
{
    public class CharacterDashActionIcon : MonoBehaviour
    {
        [SerializeField] private Image actionImage;

        private SkillComponent skillComponent;
        private AdventurerBehaviour focusedAdventurer;
        private Transform preTransform;
        private ActionBehaviour ActionBehaviour => focusedAdventurer.ActionBehaviour;
        

        public void Initialize(AdventurerBehaviour adventurer, SkillComponent skillComponent)
        {
            focusedAdventurer = adventurer;

            this.skillComponent = skillComponent;
            actionImage.sprite  = skillComponent.Icon;
        }

        public void StartAction(InputAction.CallbackContext context)
        {
            if (skillComponent.IsNullOrEmpty()) return;
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;

            ActionBehaviour.Dash(mousePosition);
        }

        public void CompleteAction(InputAction.CallbackContext context)
        {
            if (skillComponent.IsNullOrEmpty() || focusedAdventurer.IsNullOrEmpty()) return;
            
            ActionBehaviour.DashSkill.Release();
        }
    }
}
