using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class CoolTimeEntity : BaseEntity, IReadyRequired
    {
        private float remainCoolTime;

        public override bool IsReady => remainCoolTime <= 0.0f;
        public float CoolTime { get; set; }
        public float CoolTimeTick { get; set; }

        [ShowInInspector]
        public float RemainCoolTime
        {
            get => remainCoolTime;
            set => remainCoolTime = Mathf.Max(0, value);
        }

        public override void OnRegistered()
        {
            Skill.OnCompleted += SetEntity;
        }

        public override void OnUnregistered()
        {
            Skill.OnCompleted -= SetEntity;
        }
        

        protected void SetEntity() => (CoolTime, RemainCoolTime) = (SkillData.BaseCoolTime, CoolTime);
        private void UpdateStatus() => IsReady.OnFalse(() => RemainCoolTime -= CoolTimeTick);

        protected override void Awake()
        {
            base.Awake();

            SetEntity();
            CoolTimeTick = Time.deltaTime;
        }

        private void OnEnable() => Cb.OnUpdate += UpdateStatus;
        private void OnDisable() => Cb.OnUpdate -= UpdateStatus;

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            flag = EntityType.CoolTime;

            SetEntity();
        }
#endif
    }
}
