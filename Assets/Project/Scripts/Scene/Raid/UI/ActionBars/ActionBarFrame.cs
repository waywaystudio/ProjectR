using System;
using Raid.UI.ActionBars.CharacterSkills;
using UnityEngine;

namespace Raid.UI.ActionBars
{
    public class ActionBarFrame : MonoBehaviour
    {
        [SerializeField] private CharacterSkillBar CharacterSkillBar;

        private void Awake()
        {
            CharacterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
        }

        
        public void SetUp()
        {
            CharacterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
        }
    }
}
