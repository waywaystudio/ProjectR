using System.Collections.Generic;
using System.Linq;
using Character.Combat.Skill;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CombatBehaviour : MonoBehaviour, IEditorSetUp
    {
        [SerializeField] private List<BaseSkill> skillList = new();
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private Positioning positioning;
        [SerializeField] private GlobalCoolDown globalCoolDown;

        public Positioning Positioning => positioning;
        public GlobalCoolDown GlobalCoolDown => globalCoolDown;
        public List<BaseSkill> SkillList => skillList;
        public BaseSkill CurrentSkill { get; private set; }
        
        // SharedBool :: CombatBehaviorDesigner
        public bool IsCurrentSkillFinished => CurrentSkill == null || CurrentSkill.IsSkillFinished;

        // SharedBool :: CombatBehaviorDesigner
        public bool IsCoolOnAnySkill => SkillList.Any(x => x.IsCoolTimeReady);

        // ShardInt :: CombatBehaviorDesigner
        public int MostPrioritySkillID => GetMostPrioritySkillID();
        

        // TODO. 대량의 GC 발생중 (FindAll, 잦은 호출)
        public bool TryGetMostPrioritySkill(out BaseSkill skill)
        {
            var coolOnSkillList = SkillList.FindAll(x => x.IsSkillReady);
            
            if (!coolOnSkillList.IsNullOrEmpty())
            {
                skill = coolOnSkillList.MaxBy(x => x.Priority);
                return true;
            }

            skill = null;
            return skill is not null;
        }

        public void UseSkill(BaseSkill skill)
        {
            if (CurrentSkill != null) 
                CurrentSkill.DeActiveSkill();

            CurrentSkill = skill;
            CurrentSkill.ActiveSkill();

            GlobalCoolDown.StartCooling();
        }


        private int GetMostPrioritySkillID()
        {
            /*
             * Require Priority Algorithm
             */
            var mostPrioritySkillID = SkillList
                .Where(x => x.IsCoolTimeReady)
                .MaxBy(x => x.Priority).ActionCode;

            return (int)mostPrioritySkillID;
        }

        private void Awake()
        {
            positioning ??= GetComponent<Positioning>();
            globalCoolDown ??= GetComponent<GlobalCoolDown>();
            cb.CombatBehaviour = this;
        }

#if UNITY_EDITOR
        public void SetUp()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            positioning ??= GetComponent<Positioning>();
            globalCoolDown ??= GetComponent<GlobalCoolDown>();

            // TODO. 나중에는 인게임에서 캐릭터의 스킬을 변경할 수 있게 될 것이다.
            skillList = GetComponentsInChildren<BaseSkill>().Where(x => x.enabled).ToList();
        }
#endif
    }
}
