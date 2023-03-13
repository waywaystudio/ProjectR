using Adventurers;
using Raid.UI.ActionFrames.ActionBars;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private CharacterSkillBar characterSkillBar;
        [SerializeField] private AdventurerBar adventurerBars;

        private Adventurer ab;

        public void Initialize(Adventurer ab)
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
