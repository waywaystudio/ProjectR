using System;
using System.Collections;
using Character.Graphic;
using Character.TargetingSystem;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Character.Skill
{
    public class SkillComponent : MonoBehaviour, ICombatTable
    {
        /* Common Attribution */
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected AnimationModel model;
        [SerializeField] protected Targeting targeting;
        [SerializeField] private float range;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;

        /* Condition Entity */
        [SerializeField] private float coolTime;
        [SerializeField] private float cost;
        
        /* Animation */
        [SerializeField] protected string animationKey;
        
        /* Progress Entity */
        [SerializeField] protected ProcessType processType;
        [SerializeField] protected float progressTime;
        
        /* Completion Entity */
        [SerializeField] protected PowerValue powerValue;
        [SerializeField] protected DataIndex statusEffectID;
        [SerializeField] protected DataIndex projectileID;
        
        private float progressTick;
        private float coolDownTick;
        private Coroutine progressRoutine;
        private Coroutine coolTimeRoutine;
        
        /* Sequence */
        // [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public BoolTable ConditionTable { get; } = new();
        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnInterrupted { get; } = new();
        [ShowInInspector] public ActionTable OnHit { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();

        public bool OnProgress { get; private set; }
        public FloatEvent CastingProgress { get; } = new(0, float.MaxValue);
        public DataIndex ActionCode => actionCode;
        public ICombatProvider Provider { get; set; }
        public FloatEvent RemainTime { get; } = new(0f, float.MaxValue);
        public StatTable StatTable { get; } = new();
        public bool IsCompleted { get; private set; }
        public bool IsEnded { get; private set; }


        public void Active()
        {
            if (ConditionTable.HasFalse) return;
            
            OnActivated.Invoke();
        }

        public void Interrupt()
        {
            OnInterrupted.Invoke();
            OnEnded.Invoke();
        }

        public void Complete()
        {
            OnCompleted.Invoke();
            OnEnded.Invoke();
        }

        protected virtual void TryActiveSkill()
        {
            if (ConditionTable.HasFalse) return;

            OnActivated.Invoke();
        }

        protected void ConsumeResource() => Provider.DynamicStatEntry.Resource.Value -= cost;
        protected void StartCooling() => coolTimeRoutine = StartCoroutine(CoolingRoutine());
        protected void StopCooling() => (coolTimeRoutine != null).OnTrue(() => StopCoroutine(coolTimeRoutine));
        protected void StartProgress(Action callback = null)
        {
            OnProgress      = true;
            progressRoutine = StartCoroutine(Processing(callback));
        }
        
        protected void StopProcess()
        {
            if (!OnProgress) return;
            if (progressRoutine != null) StopCoroutine(progressRoutine);
            
            ResetProgress();
        }
        
        protected void UpdatePowerValue()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }
        
        protected virtual void PlayAnimation()
        {
            model.Play(animationKey, 0, false, progressTime);
        }

        private IEnumerator CoolingRoutine()
        {
            RemainTime.Value = coolTime;
            
            while (RemainTime.Value > 0f)
            {
                RemainTime.Value -= coolDownTick;
                yield return null;
            }
        }
        
        private bool IsCoolTimeReady()
        {
            if (RemainTime.Value > 0.0f) Debug.LogWarning("CoolTime is not ready");
            
            return RemainTime.Value <= 0.0f;
        }

        private bool IsCostReady()
        {
            if (Provider.DynamicStatEntry.Resource.Value < cost) Debug.LogWarning("Cost is not Enough");
            
            return Provider.DynamicStatEntry.Resource.Value >= cost;
        }

        private IEnumerator Processing(Action callback)
        {
            var endTime = progressTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            while (CastingProgress.Value < endTime)
            {
                CastingProgress.Value += progressTick;
                yield return null;
            }

            callback?.Invoke();
            ResetProgress();
        }

        private void ResetProgress()
        {
            OnProgress            = false;
            progressRoutine       = null;
            CastingProgress.Value = 0f;
        }
        

        protected virtual void Awake()
        {
            Provider     = GetComponentInParent<ICombatProvider>();
            coolDownTick = Time.deltaTime;
            progressTick = Time.deltaTime;
            
            targeting.Initialize(this);
            
            ConditionTable.Register("CoolTime", IsCoolTimeReady);
            ConditionTable.Register("Cost", IsCostReady);
            OnActivated.Register("Complete", () => IsCompleted   = false);
            OnActivated.Register("End", () => IsEnded            = false);
            OnCompleted.Register("Complete", () => IsCompleted = true);
            OnCompleted.Register("End", () => IsEnded          = true);
        }


        public void SetUp()
        {
            var skillData = MainGame.MainData.GetSkill(actionCode);

            range            = skillData.Range;
            description      = skillData.Description;
            coolTime         = skillData.BaseCoolTime;
            cost             = skillData.Cost;
            animationKey     = skillData.AnimationKey;
            progressTime     = skillData.ProcessTime;
            powerValue.Value = skillData.BaseValue;
            statusEffectID   = (DataIndex) skillData.StatusEffectId;
            projectileID     = (DataIndex) skillData.ProjectileId;
        }
        // private void ShowDB();
    }
}

