using UnityEngine;

namespace Common.Character
{
    public class Skill : MonoBehaviour
    {
        // [SerializeField] private SkillData
        /*
         * SkillData Has Main Action of "Skill";
         * actionOfType -> motionOfType;
         * targetLayer(ally, enemy, whatever...),
         * targetCount(1 or a.p.a many)
         * damage, heal
         * statusChange(buff deBuff),
         * ...priority?
         * coolTime also
         * 어떤 스킬인지에 따라서, 필요한 함수에 parameter가 달라보인다...
         */
        
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
