using System.Collections.Generic;
using UnityEngine;

namespace Common.Character
{
    using Skills;
    using Skills.Core;
    
    /*
     * CharacterBattle -> 'Class'Skill : SkillOperation
     * SkillOperation 과 직업Skill 클래스로 구분 후 각 구현.
     * 먼저 직업Skill 클래스를 작성해보고, 공통으로 만들 수 있는 것은 빼자.
     */
    public class CharacterBattle : MonoBehaviour
    {
        [SerializeField] private CommonAttack commonAttack;
        [SerializeField] private AimShot aimShot;

        private List<SkillAttribution> skillList;
        private Dictionary<int, SkillAttribution> skillTable;

        private void Update()
        {
            if (commonAttack.isActiveAndEnabled) commonAttack.UpdateStatus();
            if (aimShot.isActiveAndEnabled) aimShot.UpdateStatus();
        }
    }
}
