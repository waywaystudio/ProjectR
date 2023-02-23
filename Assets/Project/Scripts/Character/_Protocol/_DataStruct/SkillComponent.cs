using System;
using System.Collections;
using Character.Graphic;
using Character.Targeting;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character
{
    public abstract class SkillComponent : MonoBehaviour, IDataSetUp, IDataIndexer
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
        [SerializeField] protected SortingType sortingType;
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

        private Coroutine progressRoutine;
        private Coroutine coolTimeRoutine;

        /* Sequence */
        [ShowInInspector] public ConditionTable ConditionTable { get; } = new();
        [ShowInInspector] public ActionTable<Vector3> OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnInterrupted { get; } = new();
        [ShowInInspector] public ActionTable OnHit { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();

        public bool OnProgress { get; protected set; }
        public int Priority => priority;
        public float CoolTime => coolTime;
        public float Range => range;
        public float ProgressTime => progressTime;
        public string Description => description;
        public FloatEvent CastingProgress { get; } = new(0, float.MaxValue);
        public FloatEvent RemainCoolTime { get; } = new(0f, float.MaxValue);
        public Sprite Icon => icon;
        public DataIndex ActionCode => actionCode;
        public ICombatProvider Provider { get; set; }
        public ICombatTaker MainTarget => searching.GetMainTarget(targetLayer, Provider.Object.transform.position, sortingType);

        public void Activate(Vector3 targetPosition)
        {
            if (ConditionTable.HasFalse) return;

            OnProgress = true;
            OnActivated.Invoke(targetPosition);
        }

        public void Interrupted()
        {
            if (!OnProgress) return;

            OnInterrupted.Invoke();
        }

        public virtual void Release()
        {
            if (!OnProgress) return;
            
            OnCompleted.Invoke();
        }


        protected void ConsumeResource() => Provider.DynamicStatEntry.Resource.Value -= cost;
        protected void StartCooling() => coolTimeRoutine = StartCoroutine(CoolingRoutine());
        protected void StartProcess(Action callback = null)
        {
            progressRoutine = StartCoroutine(Processing(callback));
        }
        
        protected void StopProcess()
        {
            if (progressRoutine != null) StopCoroutine(progressRoutine);
            
            ResetProcess();
        }

        protected virtual void PlayAnimation()
        {
            model.PlayOnce(animationKey, progressTime);
        }
        
        protected bool IsCoolTimeReady()
        {
            // if (RemainCoolTime.Value > 0.0f) Debug.LogWarning("CoolTime is not ready");
            return RemainCoolTime.Value <= 0.0f;
        }

        protected bool IsCostReady()
        {
            // if (Provider.DynamicStatEntry.Resource.Value < cost) Debug.LogWarning("Not Enough Cost");
            return Provider.DynamicStatEntry.Resource.Value >= cost;
        }

        private IEnumerator CoolingRoutine()
        {
            RemainCoolTime.Value = coolTime;

            while (RemainCoolTime.Value > 0f)
            {
                RemainCoolTime.Value -= Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Processing(Action callback)
        {
            var endTime = progressTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            while (CastingProgress.Value < endTime)
            {
                CastingProgress.Value += Time.deltaTime;
                yield return null;
            }

            callback?.Invoke();
            ResetProcess();
        }

        private void ResetProcess()
        {
            progressRoutine       = null;
            CastingProgress.Value = 0f;
        }

        protected virtual void Awake()
        {
            Provider     = GetComponentInParent<ICombatProvider>();
            
            ConditionTable.Register("CoolTime", IsCoolTimeReady);
            ConditionTable.Register("Cost", IsCostReady);
            OnEnded.Register("EndProgress", () => OnProgress = false);
        }


#if UNITY_EDITOR
        public virtual void SetUp()
        {
            var skillData = MainData.SkillSheetData(actionCode);
            var provider = GetComponentInParent<ICombatProvider>();
            
            priority     = skillData.BasePriority;
            range        = skillData.TargetParam.x;
            angle        = skillData.TargetParam.y;
            description  = skillData.Description;
            coolTime     = skillData.CoolTime;
            cost         = skillData.Cost;
            animationKey = skillData.AnimationKey;
            progressTime = skillData.ProcessTime;
            sortingType  = skillData.SortingType.ToEnum<SortingType>();
            
            // TODO. 프리팹 상태에서 Provider를 가지고 올 수 없기 때문에 문제가 될 수 있다. 
            targetLayer  = CharacterUtility.SetLayer(provider, skillData.TargetLayer);
            icon         = GetSkillIcon();
        }
        // private void ShowDB(); 
        
        private Sprite GetSkillIcon()
        {
            return !(MainData.TryGetIcon(actionCode.ToString(), out var result))
                ? null
                : result;
        }
#endif
    }
}
