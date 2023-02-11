using System.Collections.Generic;
using Character.Combat.Skill;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class OldSkillBehaviour : MonoBehaviour, ISkillBehaviour, IInspectorSetUp
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private OldGlobalCoolDown globalCoolDown;
        [SerializeField] private SkillTable skillTable;

        private int instanceID;

        public ICombatProvider Provider => cb;
        public SkillObject CurrentSkill { get; private set; }
        public List<ISkillInfo> SkillInfoList => skillTable.SelectSkillInfo;
        public List<SkillObject> SkillList => skillTable.SelectSkillList;
        public float GlobalCoolTime => globalCoolDown.CoolTime;
        public OldObservable<float> GlobalRemainTime => globalCoolDown.Timer;
        public bool IsCurrentSkillFinished => CurrentSkill == null || CurrentSkill.IsSkillFinished;

        public OldActionTable<SkillObject> OnUseSkill { get; } = new(4);

        public void UseSkill(SkillObject skill)
        {
            UnregisterSkill();
            SetCurrentSkill(skill);
            RegisterSkill();
            
            cb.ActiveSkill();
            globalCoolDown.StartCooling();
        }
        
        // TODO. GC 발생중 (잦은 호출) -> 많이 수정 함
        public bool TryGetMostPrioritySkill(out SkillObject skill)
        {
            skill = null;

            for (var i = 0; i < SkillList.Count; ++i)
            {
                if (!SkillList[i].IsSkillReady) continue;
                
                skill ??= SkillList[i];
                if (SkillList[i].Priority >= skill.Priority) skill = SkillList[i];
            }
            
#if UNITY_EDITOR
            if (skill is null)
            {
                // TODO. RayCastModule 이 기초스킬일 경우, 로그가 띄워질 수 있다.
                Debug.LogWarning("캐릭터 스킬 구성에는 항상 준비되어 있는 기초스킬을 가지고 있다."
                                 + "이 로그가 띄워지면, 기초스킬 구성이 잘못 되어 있거나, 빠져있을 수 있다.");
            }
#endif
            return skill is not null;
        }


        private void SetCurrentSkill(SkillObject skill)
        {
            CurrentSkill = skill;
            cb.SkillInfo = skill;
        }
        
        private void RegisterSkill()
        {
            cb.OnActiveSkill.Register(CurrentSkill.InstanceID, CurrentSkill.Active);
            cb.OnHitSkill.Register(CurrentSkill.InstanceID, CurrentSkill.Hit);
            cb.OnCompleteSkill.Register(CurrentSkill.InstanceID, CurrentSkill.Complete);
            cb.OnCancelSkill.Register(CurrentSkill.InstanceID, CurrentSkill.Cancel);
        }

        private void UnregisterSkill()
        {
            if (CurrentSkill is null) return;
            
            cb.OnActiveSkill.Unregister(CurrentSkill.InstanceID);
            cb.OnHitSkill.Unregister(CurrentSkill.InstanceID);
            cb.OnCompleteSkill.Unregister(CurrentSkill.InstanceID);
            cb.OnCancelSkill.Unregister(CurrentSkill.InstanceID);
            
            cb.SkillInfo = null;
            CurrentSkill = null;
        }

        private void Awake()
        {
            cb                 ??= GetComponentInParent<CharacterBehaviour>();
            globalCoolDown     ??= GetComponent<OldGlobalCoolDown>();
            instanceID         =   GetInstanceID();

            OnUseSkill.Register(instanceID, UseSkill);
            cb.OnUseSkill.Register(instanceID, OnUseSkill.Invoke);
            SkillInfoList.AddRange(SkillList);
        }
        

#if UNITY_EDITOR
        public void SetUp()
        {
            cb             ??= GetComponentInParent<CharacterBehaviour>();
            globalCoolDown ??= GetComponent<OldGlobalCoolDown>();
            skillTable     ??= GetComponentInChildren<SkillTable>();
        }
#endif
    }
}
