using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class CoolTimeEntity : BaseEntity
    {
        [SerializeField] private float coolTime;
        private float remainTimer;

        public override bool IsReady => remainTimer <= 0.0f;
        public float CoolTime { get => coolTime; set => coolTime = value; }
        public float CoolTimeTick { get; set; }
        [ShowInInspector]
        public float RemainTimer
        {
            get => remainTimer;
            set => remainTimer = Mathf.Max(0, value);
        }


        public override void SetEntity()
        {
            CoolTime = coolTime;
            RemainTimer = CoolTime;
        }
        
        private void UpdateStatus() => IsReady.OnFalse(() => RemainTimer -= CoolTimeTick);
        private void ResetRemainTime() => RemainTimer = CoolTime;

        protected override void Awake()
        {
            base.Awake();
            
            CoolTime = coolTime;
            RemainTimer = 0f;
            CoolTimeTick = Time.deltaTime;
        }

        private void OnEnable()
        {
            Cb.OnUpdate.Register(InstanceID, UpdateStatus);
            AssignedSkill.OnCompleted.Register(InstanceID, ResetRemainTime);
        }
        
        private void OnDisable()
        { 
            Cb.OnUpdate.Unregister(InstanceID);
            AssignedSkill.OnCompleted.Unregister(InstanceID);
        }

        private void Reset()
        {
            flag = EntityType.CoolTime;
            
            var skillData = MainGame.MainData.GetSkillData(GetComponent<BaseSkill>().ActionName);
            coolTime = skillData.BaseCoolTime;
            RemainTimer = 0f;
        }
    }
}
