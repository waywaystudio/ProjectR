using Character;
using Character.Actions;
using Character.Skill;
using Core;
using MainGame;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.ActionFrames.ActionBars.CharacterSkillBars
{
    public class CharacterDashActionIcon : MonoBehaviour
    {
        // [SerializeField] private Image actionImage;
        //
        // private SkillComponent skillComponent;
        // private Adventurer focusedAdventurer;
        // private Transform preTransform;
        // private ActionBehaviour ActionBehaviour => focusedAdventurer.ActionBehaviour;
        //
        //
        // public void Initialize(Adventurer adventurer, SkillComponent skillComponent)
        // {
        //     focusedAdventurer = adventurer;
        //
        //     this.skillComponent = skillComponent;
        //     actionImage.sprite  = skillComponent.Icon;
        // }
        //
        // public void StartAction(InputAction.CallbackContext context)
        // {
        //     if (skillComponent.IsNullOrEmpty()) return;
        //     if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;
        //
        //     ActionBehaviour.Dash(mousePosition);
        // }
        //
        // public void CompleteAction(InputAction.CallbackContext context)
        // {
        //     if (skillComponent.IsNullOrEmpty() || focusedAdventurer.IsNullOrEmpty()) return;
        //     
        //     ActionBehaviour.DashSkill.Release();
        // }
    }
}
