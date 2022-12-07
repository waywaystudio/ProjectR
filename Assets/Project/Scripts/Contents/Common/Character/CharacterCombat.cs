using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    using Skills;
    using Skills.Core;
    using Skills.Entity;
    
    /*
     * CharacterBattle -> 'Class'Skill : SkillOperation
     * CharacterBehavior를 Variant로 만들어야 하는 필요조건 파트.
     * SkillOperation 과 직업Skill 클래스로 구분 후 각 구현.
     * 먼저 직업Skill 클래스를 작성해보고, 공통으로 만들 수 있는 것은 빼자.
     */
    public class CharacterCombat : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [ShowInInspector] private SkillAttribution nextSkill;

        public CharacterBehaviour Cb => cb;
        public SkillAttribution NextSKill => nextSkill;
    }
}
