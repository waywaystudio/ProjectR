using System;
using Common.Character.Skills;
using UnityEngine;

namespace Common.Character
{
    public class CharacterBattle : MonoBehaviour
    {
        // SkillDataList;
        // each coolTime tick;
        // Update
        [SerializeField] private CommonAttack commonAttack;
        
        // Global CoolDown Packages;
        // private Skill(?) nextSkill;
        // public void DoSkill() => nextSkill.Invoke();

        private void Update()
        {
            // CommonAttack Update
            if (!commonAttack.IsCoolOn)
            {
                // commonAttack.DecreaseCoolTime();
            }
        }
    }
}
