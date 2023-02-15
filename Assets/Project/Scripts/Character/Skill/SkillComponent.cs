using System;
using System.Collections;
using Character.Graphic;
using Character.Targeting;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Skill
{
    public abstract class SkillComponent : MonoBehaviour, IInspectorSetUp
    {
        /* Component Reference */
        [SerializeField] protected AnimationModel model;
        [SerializeField] protected Searching searching;
        [SerializeField] protected Colliding colliding;
        
        /* Common Attribution */
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected int priority;
        [SerializeField] protected float range;
        [SerializeField] protected float angle;
        [SerializeField] protected LayerMask targetLayer;
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

        private float progressTick;
        private float coolDownTick;
        private Coroutine progressRoutine;
        private Coroutine coolTimeRoutine;

        /* Sequence */
        [ShowInInspector] public ConditionTable ConditionTable { get; } = new();
        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnInterrupted { get; } = new();
        [ShowInInspector] public ActionTable OnHit { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();

        public bool OnProgress { get; private set; }
        public bool IsEnded { get; private set; } = true;
        public int Priority => priority;
        public float Range => range;
        public FloatEvent CastingProgress { get; } = new(0, float.MaxValue);
        public FloatEvent RemainTime { get; } = new(0f, float.MaxValue);
        public Sprite Icon => icon;
        public DataIndex ActionCode => actionCode;
        public ICombatProvider Provider { get; set; }
        public ICombatTaker MainTarget => searching.GetMainTarget(targetLayer, Provider.Object.transform.position);


        public void Active()
        {
            if (ConditionTable.HasFalse) return;

            TryActiveSkill();
        }

        public void Interrupt()
        {
            OnInterrupted.Invoke();
            OnEnded.Invoke();
        }

        protected virtual void TryActiveSkill() => OnActivated.Invoke();
        protected void ConsumeResource() => Provider.DynamicStatEntry.Resource.Value -= cost;
        protected void StartCooling() => coolTimeRoutine = StartCoroutine(CoolingRoutine());
        protected void StopCooling() => (coolTimeRoutine != null).OnTrue(() => StopCoroutine(coolTimeRoutine));
        protected void StartProcess(Action callback = null)
        {
            OnProgress      = true;
            progressRoutine = StartCoroutine(Processing(callback));
        }
        
        protected void StopProcess()
        {
            if (!OnProgress) return;
            if (progressRoutine != null) StopCoroutine(progressRoutine);
            
            ResetProcess();
        }

        protected abstract void UpdateCompletion();
        protected virtual void PlayAnimation()
        {
            model.PlayOnce(animationKey, progressTime);
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
            ResetProcess();
        }

        private void ResetProcess()
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

            ConditionTable.Register("CoolTime", IsCoolTimeReady);
            ConditionTable.Register("Cost", IsCostReady);
            OnActivated.Register("End", () => IsEnded = false);
            OnEnded.Register("End", () => IsEnded = true);
        }


        public virtual void SetUp()
        {
            var skillData = MainGame.MainData.GetSkill(actionCode);
            
            priority     = skillData.BasePriority;
            range        = skillData.TargetParam1;
            angle        = skillData.TargetParam2;
            description  = skillData.Description;
            coolTime     = skillData.CoolTime;
            cost         = skillData.Cost;
            animationKey = skillData.AnimationKey;
            progressTime = skillData.ProcessTime;
        }
        // private void ShowDB();
    }
}

