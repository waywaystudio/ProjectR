using System.Collections.Generic;
using System.Linq;
using Character.Combat.Skill;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CombatBehaviour : MonoBehaviour, ICombatBehaviour, IEditorSetUp
    {
        [SerializeField] private List<BaseSkill> skillList = new(4);
        [SerializeField] private Positioning positioning;
        [SerializeField] private GlobalCoolDown globalCoolDown;

        public Positioning Positioning => positioning;
        public GlobalCoolDown GlobalCoolDown => globalCoolDown;
        
        // TODO. ISKillInfo쪽을 다듬으면, 아래 필드를 삭제할 수도 있다.
        public IEnumerable<BaseSkill> SkillList => skillList;
        public BaseSkill CurrentSkill { get; private set; }

        public float GlobalCoolTime => GlobalCoolDown.CoolTime;
        public List<ISkillInfo> SkillInfoList { get; set; } = new(4);
        public Observable<float> GlobalRemainTime => GlobalCoolDown.Timer;
        
        // SharedBool :: CombatBehaviorDesigner
        public bool IsCurrentSkillFinished => CurrentSkill == null || CurrentSkill.IsSkillFinished;

        // SharedBool :: CombatBehaviorDesigner
        public bool IsCoolOnAnySkill => SkillList.Any(x => x.IsCoolTimeReady);

        // ShardInt :: CombatBehaviorDesigner
        public int MostPrioritySkillID => GetMostPrioritySkillID();
        

        // TODO. 대량의 GC 발생중 (잦은 호출) -> 많이 수정 함
        public bool TryGetMostPrioritySkill(out BaseSkill skill)
        {
            skill = null;
            
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < skillList.Count; ++i)
            {
                if (!skillList[i].IsSkillReady) continue;
                
                skill ??= skillList[i];
                if (skillList[i].Priority >= skill.Priority) skill = skillList[i];
            }

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


        // TODO. 대량의 GC 발생중 (FindAll, 잦은 호출)
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
            GetComponentInParent<CharacterBehaviour>().CombatBehaviour = this;
            
            positioning ??= GetComponent<Positioning>();
            globalCoolDown ??= GetComponent<GlobalCoolDown>();
            SkillInfoList.AddRange(SkillList);
        }

#if UNITY_EDITOR
        public void SetUp()
        {
            positioning ??= GetComponent<Positioning>();
            globalCoolDown ??= GetComponent<GlobalCoolDown>();

            // TODO. 나중에는 인게임에서 캐릭터의 스킬을 변경할 수 있게 될 것이다.
            skillList = GetComponentsInChildren<BaseSkill>().Where(x => x.enabled).ToList();
        }
#endif
    }
}
