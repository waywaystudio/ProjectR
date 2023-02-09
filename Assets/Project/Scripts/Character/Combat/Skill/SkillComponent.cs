using System.Collections;
using Character.Graphic;
using Character.TargetingSystem;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class SkillComponent : MonoBehaviour, ICombatTable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected SkillData data;
        [SerializeField] protected AnimationModel model;
        
        public DataIndex ActionCode => actionCode;
        public ICombatProvider Provider { get; set; }
        
        /* Common Attribution */
        [SerializeField] private float range;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;

        public bool IsCompleted { get; protected set; }

        /* Sequence */
        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnCanceled { get; } = new();
        [ShowInInspector] public ActionTable OnHit { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();

        /* Target Entity */
        [SerializeField] protected Targeting targeting;
        
        /* Condition Entity */
        [SerializeField] private float coolTime;
        [SerializeField] private float cost;
        
        private float coolDownTick;
        private Coroutine coolTimeRoutine;
        public BoolTable IsReady { get; } = new();
        public FloatEvent RemainTime { get; } = new(0, float.MaxValue);
        private bool IsCoolTimeReady => coolTime != 0f && RemainTime.Value <= 0.0f;
        private bool IsCostReady => cost != 0f && Provider.DynamicStatEntry.Resource.Value >= cost;
        
        public void ConsumeResource() => Provider.DynamicStatEntry.Resource.Value -= cost;
        public void StartCooling() => coolTimeRoutine = StartCoroutine(CoolingRoutine());
        public void StopCooling() => (coolTimeRoutine != null).OnTrue(() => StopCoroutine(coolTimeRoutine));

        private IEnumerator CoolingRoutine()
        {
            RemainTime.Value = coolTime;

            while (RemainTime.Value > 0)
            {
                RemainTime.Value -= coolDownTick;
                yield return null;
            }

            RemainTime.Value = 0f;
        }
        
        /* Animation */
        [SerializeField] protected string animationKey;
        
        protected virtual void PlayAnimation()
        {
            model.Play(animationKey, 0, false, progressTime/*, OnCompleted.Invoke*/);
        }
        
        
        /* Progress Entity */
        [SerializeField] protected ProgressType progressType;
        [SerializeField] protected float progressTime;
        
        private float progressTick;
        private Coroutine progressRoutine;
        
        public bool OnProgress { get; private set; }
        public FloatEvent CastingProgress { get; } = new(0, float.MaxValue);
        
        public void StartProgress()
        {
            OnProgress       = true;
            progressRoutine = StartCoroutine(Progressing());
        }
        
        private IEnumerator Progressing()
        {
            var endTime = progressTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            while (CastingProgress.Value < endTime)
            {
                CastingProgress.Value += progressTick;
                yield return null;
            }
            
            ResetProgress();
        }
        
        private void BreakProcess()
        {
            if (!OnProgress) return;
            if (progressRoutine != null) StopCoroutine(progressRoutine);
            
            ResetProgress();
        }
        
        private void ResetProgress()
        {
            OnProgress             = false;
            progressRoutine       = null;
            CastingProgress.Value = 0f;
        }
        
        /* Completion Entity */
        [SerializeField] protected PowerValue powerValue;
        [SerializeField] protected DataIndex statusEffectID;
        [SerializeField] protected DataIndex projectileID;
        
        public StatTable StatTable { get; } = new();

        protected void UpdatePowerValue()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }

        public virtual void UseSkill()
        {
            if (!IsReady.IsAllTrue()) return;

            OnActivated.Invoke();
        }

        // protected virtual void OnEnable()
        // {
        //     OnActivated.Register("PlayAnimation", PlayAnimation);
        //     OnActivated.Register("SkillFinished", () => IsFinished = false);
        //     OnCanceled.Register("CancelAnimation", animationModel.Idle);
        //     OnCompleted.Register("IdleAnimation", animationModel.Idle);
        //     OnCompleted.Register("SkillFinished", () => IsFinished = true);
        //     
        //     // PlayerController.Mouseclick.performed += OnCanceled.Invoke;
        // }

        protected virtual void Awake()
        {
            Provider     = GetComponentInParent<ICombatProvider>();
            coolDownTick = Time.deltaTime;
            progressTick = Time.deltaTime;
            
            targeting.Initialize(this);
            
            IsReady.Register("CoolTime", () => IsCoolTimeReady);
            IsReady.Register("Cost", () => IsCostReady);
        }
        

        public void SetUp()
        {
            data.SetUp(ActionCode);
        }
        
        // private void ShowDB();
    }
}

