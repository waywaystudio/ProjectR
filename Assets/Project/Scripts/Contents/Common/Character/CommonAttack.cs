using Data.ContentData;
using UnityEngine;

namespace Common.Character
{
    public class CommonAttack : MonoBehaviour
    {
        /*
         * SkillData Has Main Action of "Skill";
         * actionOfType -> motionOfType;
         * targetCount(1 or apa many)
         * range
         * ...priority?
         * coolTime also
         *
         * * 아래 데이터는 받아서 뭘 생성하지는 않을꺼 같다.
         * targetLayer(ally, enemy, self, whatever...),         
         * damage, heal, statusChange(buff deBuff),         
         * 어떤 스킬인지에 따라서, 필요한 함수에 parameter가 달라보인다...
         */

        [SerializeField] private SkillData skillData;
        private float remainCoolTime;
        
        public bool IsReady => remainCoolTime <= 0.0f;
        public string SkillName { get; private set; }
        public float CoolTime { get; private set; }

        public void Initialize(string skillName)
        {
            // var assignedSkill = SkillData.GetSkillByName(skillName);
            // SkillName = assignedSkill.skillName;
            // CoolTime = assignedSkill.baseCoolTime;
        }

        public void DoSkill(GameObject target)
        {
            if (!IsReady)
            {
                Debug.Log("Not Ready");
                return;
            }

            remainCoolTime = CoolTime;
        }
    }
}
