using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Common.Character.Operation
{
    using Combat;
    
    public class CombatOperation : MonoBehaviour
    {
        public BaseSkill FirstSkill;
        public BaseSkill SecondSkill;
        public BaseSkill ThirdSkill;
        public BaseSkill FourthSkill;
        
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private Positioning positioning;
        [SerializeField] private GlobalCoolDown globalCoolDown;

        public Positioning Positioning => positioning;
        public GlobalCoolDown GlobalCoolDown => globalCoolDown;

        public Dictionary<int, BaseSkill> SkillTable { get; } = new();
        public BaseSkill CurrentSkill { get; set; }
        public bool IsCurrentSkillFinished => CurrentSkill == null || CurrentSkill.IsSkillFinished;

        // SharedBool :: CombatBehaviorDesigner
        public bool IsCoolOnAnySkill => SkillTable.Any(x => x.Value.IsCoolTimeReady);

        // ShardInt :: CombatBehaviorDesigner
        public int MostPrioritySkillID => GetMostPrioritySkillID();
        

        public bool TryGetMostPrioritySkill(out BaseSkill skill)
        {
            var coolOnSkill = SkillTable.Where(x => x.Value.IsSkillReady)
                .ToList();
            
            if (!coolOnSkill.IsNullOrEmpty())
            {
                skill = coolOnSkill.MaxBy(x => x.Value.Priority).Value;
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

            GlobalCoolDown.CoolOn();
        }


        private int GetMostPrioritySkillID()
        {
            /*
             * Require Priority Algorithm
             */
            var mostPrioritySkillID = SkillTable
                .Where(x => x.Value.IsCoolTimeReady)
                .MaxBy(x => x.Value.Priority).Value.ActionCode;

            return (int)mostPrioritySkillID;
        }

        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            cb.CombatOperation = this;
            positioning ??= GetComponent<Positioning>();
            globalCoolDown ??= GetComponent<GlobalCoolDown>();

            SkillTable.TryAdd((int)FirstSkill.ActionCode, FirstSkill);
            SkillTable.TryAdd((int)SecondSkill.ActionCode, SecondSkill);
            SkillTable.TryAdd((int)ThirdSkill.ActionCode, ThirdSkill);
            SkillTable.TryAdd((int)FourthSkill.ActionCode, FourthSkill);
        }

#if UNITY_EDITOR
        private void SetUp()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            positioning ??= GetComponent<Positioning>();
            globalCoolDown ??= GetComponent<GlobalCoolDown>();

            
            // TODO. 나중에는 인게임에서 캐릭터의 스킬을 변경할 수 있게 될 것이다.
            var skillList = GetComponentsInChildren<BaseSkill>().Where(x => x.enabled).ToList();
            (skillList.Count >= 1).OnTrue(() => FirstSkill = skillList[0]);
            (skillList.Count >= 2).OnTrue(() => SecondSkill = skillList[1]);
            (skillList.Count >= 3).OnTrue(() => ThirdSkill = skillList[2]);
            (skillList.Count >= 4).OnTrue(() => FourthSkill = skillList[3]);
            //
        }
#endif
    }
}
