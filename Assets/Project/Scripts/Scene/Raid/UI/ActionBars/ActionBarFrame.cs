using Character;
using UnityEngine;

namespace Raid.UI.ActionBars
{
    using CharacterSkills;
    
    public class ActionBarFrame : MonoBehaviour
    {
        [SerializeField] private AdventurerBehaviour ab;
        [SerializeField] private CharacterSkillBar CharacterSkillBar;

        private void Awake()
        {
            CharacterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
            CharacterSkillBar.Initialize(ab);
        }

        
        public void SetUp()
        {
            CharacterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
        }
    }
}
