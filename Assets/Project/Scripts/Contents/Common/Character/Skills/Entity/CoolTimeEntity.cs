using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class CoolTimeEntity : EntityAttribution, ICoolTimeEntity, IUpdateRequired ,IReadyRequired
    {   
        [SerializeField] private float coolTime;

        private float remainCoolTime;
        private float tick;

        public bool IsReady => remainCoolTime <= 0.0f;
        public float CoolTime { get => coolTime; set => coolTime = value; }
        public float CoolTimeTick { get => tick; set => tick = value; }
        public float RemainCoolTime
        {
            get => remainCoolTime;
            set => remainCoolTime = Mathf.Max(0, value);
        }

        public void UpdateStatus()
        {
            if (IsReady) return;
            
            RemainCoolTime -= tick;
        }

        private void Awake()
        {
            Flag = EntityType.CoolTime;
            tick = Time.deltaTime;
        }

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            base.OnEditorInitialize();
            
            Flag = EntityType.CoolTime;
            CoolTime = StaticData.BaseCoolTime;
        }
#endif
    }
}
