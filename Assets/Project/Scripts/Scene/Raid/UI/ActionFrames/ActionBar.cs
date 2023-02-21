using Character;
using Raid.UI.ActionFrames.ActionBars;
using Raid.UI.ActionFrames.ActionBars.AdventurerBars;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private CharacterSkillBar characterSkillBar;
        [SerializeField] private AdventurerBar adventurerBars;

        private AdventurerBehaviour ab;

        public void Initialize(AdventurerBehaviour ab)
        {
            this.ab = ab;
            
            characterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
            characterSkillBar.Initialize(ab);
        }


        public void SetUp()
        {
            characterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
        }
    }
}
