using UnityEngine;

namespace Raid.UI.ActionBars.CharacterSkillBar
{
    public class CharacterSkillActionSlot : MonoBehaviour
    {
        [SerializeField] private CharacterSkillActionIcon skillAction;
        [SerializeField] private string shortCutKey;

        public CharacterSkillActionIcon SkillAction => skillAction;

        public void Register(CharacterSkillActionIcon skillAction)
        {
            this.skillAction = skillAction;
        }

        public void Unregister() => skillAction = null;

        // OnClick
        // PressShortCut
        
        /// OnBeginDrag
        /// OnDrag
        /// OnEndDrag
    }
}
