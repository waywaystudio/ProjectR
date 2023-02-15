using Character;
using Core;
using MainGame;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.ActionBars.CharacterSkills
{
    public class CharacterSkillActionSlot : MonoBehaviour
    {
        [SerializeField] private CharacterSkillActionIcon skillAction;
        [SerializeField] private BindingCode bindingCode;
        [SerializeField] private int tempSkillIndex;

        private Image image;
        private AdventurerBehaviour focusedAdventurer;
        private DataIndex actionCode;
        

        public void ChangeSkill()
        {
            if (!MainManager.Input.TryGet(bindingCode, out var inputAction)) return;

            var skillList = focusedAdventurer.ActionBehaviour.SkillList;
            if (skillList.Count < tempSkillIndex || skillList[tempSkillIndex].IsNullOrEmpty()) return;

            var skillComponent = skillList[tempSkillIndex];
            
            actionCode   = skillComponent.ActionCode;
            image.sprite = skillComponent.Icon;
            
            
        }

        // public CharacterSkillActionIcon SkillAction => skillAction;

        public void Register(CharacterSkillActionIcon skillAction)
        {
            // this.skillAction = skillAction;
        }

        private void Awake()
        {
            TryGetComponent(out image);

            skillAction ??= GetComponentInChildren<CharacterSkillActionIcon>();
        }

        // public void Unregister() => skillAction = null;

        // OnClick
        // PressShortCut
        
        /// OnBeginDrag
        /// OnDrag
        /// OnEndDrag
    }
}
