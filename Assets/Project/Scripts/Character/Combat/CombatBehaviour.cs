using System.Collections.Generic;
using System.Linq;
using Character.Combat.Skill;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CombatBehaviour : MonoBehaviour, ICombatBehaviour, IEditorSetUp
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private List<SkillObject> skillList = new(4);
        [SerializeField] private GlobalCoolDown globalCoolDown;
        // [SerializeField] private SkillTable skillTable;

        // ISkillTable skillTable;
        public GlobalCoolDown GlobalCoolDown => globalCoolDown;
        // Priority Algorithm.
        
        // TODO. ISKillInfo쪽을 다듬으면, 아래 필드를 삭제할 수도 있다.
        public IEnumerable<SkillObject> SkillList => skillList;
        public SkillObject CurrentSkill { get; private set; }
        public List<ISkill> SkillInfoList { get; } = new(4);
        
        public float GlobalCoolTime => GlobalCoolDown.CoolTime;
        public Observable<float> GlobalRemainTime => GlobalCoolDown.Timer;
        
        // SharedBool :: CombatBehaviorDesigner
        public bool IsCurrentSkillFinished => CurrentSkill == null || CurrentSkill.IsSkillFinished;

        // SharedBool :: CombatBehaviorDesigner
        public bool IsCoolOnAnySkill => SkillList.Any(x => x.IsCoolTimeReady);

        public ActionTable<ISkill> OnActiveSkill { get; } = new();
        public ActionTable<ISkill> OnCompleteSkill { get; } = new();
        public ActionTable OnHitSkill { get; } = new();

        public void StartSkill(ISkill skill)
        {
            DeActiveSkill();
            
            cb.SkillInfo = skill;
            // skillTable.StartSkill(skill);
            // globalCoolTime.ActiveSkill();
        }

        public void CompleteSkill(ISkill skill)
        {
            // skillTable.CompleteSkill(skill);
        }
        
        public void HitSkill(ISkill skill)
        {
            // skill.Hit();
        }

        public void CancelSkill(ISkill skill)
        {
            // skill.Cancel();
        }
        
        public void UseSkill(SkillObject skill)
        {
            if (CurrentSkill != null) 
                CurrentSkill.DeActiveSkill();

            CurrentSkill = skill;
            CurrentSkill.ActiveSkill();

            GlobalCoolDown.StartCooling();
        }
        

        private void DeActiveSkill()
        {
            cb.SkillInfo = null;
        }


        // TODO. 대량의 GC 발생중 (잦은 호출) -> 많이 수정 함
        public bool TryGetMostPrioritySkill(out SkillObject skill)
        {
            skill = null;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < skillList.Count; ++i)
            {
                if (!skillList[i].IsSkillReady) continue;
                
                skill ??= skillList[i];
                if (skillList[i].Priority >= skill.Priority) skill = skillList[i];
            }
            
#if UNITY_EDITOR
            if (skill == null)
            {
                // TODO. RayCastModule 이 기초스킬일 경우, 로그가 띄워질 수 있다.
                Debug.LogWarning("캐릭터 스킬 구성에는 항상 준비되어 있는 기초스킬을 가지고 있다."
                                 + "이 로그가 띄워지면, 기초스킬 구성이 잘못 되어 있거나, 빠져있을 수 있다.");
            }
#endif
            return skill is not null;
        }
        

        private void Awake()
        {
            cb                 ??= GetComponentInParent<CharacterBehaviour>();
            cb.CombatBehaviour =   this;
            
            globalCoolDown ??= GetComponent<GlobalCoolDown>();
            SkillInfoList.AddRange(SkillList);
        }
        

#if UNITY_EDITOR
        public void SetUp()
        {
            globalCoolDown ??= GetComponent<GlobalCoolDown>();

            // TODO. 나중에는 인게임에서 캐릭터의 스킬을 변경할 수 있게 될 것이다.
            skillList = GetComponentsInChildren<SkillObject>().Where(x => x.enabled).ToList();
        }
#endif
    }
}
