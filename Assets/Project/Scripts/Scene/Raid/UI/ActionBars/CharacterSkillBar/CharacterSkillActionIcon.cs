using Character;
using Core;
using UnityEngine;
using SkillData = MainGame.Data.CombatData.SkillData.Skill;

namespace Raid.UI.ActionBars.CharacterSkillBar
{
    public class CharacterSkillActionIcon : MonoBehaviour
    {
        [SerializeField] private DataIndex skillCode;
        [SerializeField] private Sprite icon;
        
        /* 결국 개별적으로 다 가지고 있어야 하나...for information(=tooltip)
        [SerializeField] private SkillData skillData;
        or
        [SerializeField] private SkillComponent skillComponent;
        */

        private AdventurerBehaviour focusedAdventure;
        

        public void Initialize()
        {
            
        }

        public void Active()
        {
            if (focusedAdventure.IsNullOrEmpty())
            {
                Debug.LogWarning("Skill Owner Missing.");
            }
            
            focusedAdventure.ActiveSkill(skillCode);
        }
        
        // OnBeginDrag
        // OnDrag
        // OnEndDrag
    }
}
