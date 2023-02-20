using Character;
using UnityEngine;

namespace Raid.UI.ActionBars
{
    using CharacterSkills;
    
    public class ActionBarFrame : MonoBehaviour
    {
        [SerializeField] private CharacterSkillBar CharacterSkillBar;

        private AdventurerBehaviour ab;

        public void Initialize(AdventurerBehaviour ab)
        {
            this.ab = ab;
            
            CharacterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
            CharacterSkillBar.Initialize(ab);
        }


        public void SetUp()
        {
            CharacterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
        }
    }
}
