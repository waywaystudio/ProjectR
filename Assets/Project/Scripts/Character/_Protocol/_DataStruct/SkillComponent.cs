using System;
using System.Collections;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character
{
    public abstract class SkillComponent : MonoBehaviour, ISequence, IEditable, IDataIndexer
    {
        /* Common Attribution */
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected int priority;
        [SerializeField] protected float range;
        [SerializeField] protected float angle;
        [SerializeField] protected SortingType sortingType;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;

        /* Condition Entity */
        [SerializeField] private bool isRigid;
        [SerializeField] private float coolTime;
        [SerializeField] private float cost;
        
        /* Animation */
        [SerializeField] protected string animationKey;
        
        /* Progress Entity */
        [SerializeField] protected float progressTime;

        protected ICharacterSystem CharacterSystem;
        private Coroutine progressRoutine;
        private Coroutine coolTimeRoutine;

        /* Sequence */
        [ShowInInspector] public ConditionTable ConditionTable { get; } = new();
        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnCanceled { get; } = new();
        [ShowInInspector] public ActionTable OnHit { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();

        public SkillType SkillType => skillType;
        public bool IsRigid => isRigid;
        public bool IsEnded { get; protected set; } = true;
        public int Priority => priority;
        public float CoolTime => coolTime;
        public float Range => range;
        public float Angle => angle;
        public float ProgressTime => progressTime;
        public string Description => description;
        public FloatEvent CastingProgress { get; } = new(0, float.MaxValue);
        public FloatEvent RemainCoolTime { get; } = new(0f, float.MaxValue);
        public LayerMask TargetLayer => targetLayer;
        public Sprite Icon => icon;
        public DataIndex ActionCode => actionCode;
        public ICombatProvider Provider { get; set; }
        public virtual ICombatTaker MainTarget => 
            CharacterSystem.Searching.GetMainTarget(targetLayer, Provider.Object.transform.position, sortingType);
        
        protected bool IsProgress { get; set; }


        public void Activate()
        {
            IsProgress = true;
            CharacterSystem.Pathfinding.Stop();
            OnActivated.Invoke();
        }

        public void Cancel()
        {
            OnCanceled.Invoke();
        }
        
        public void Release()
        {
            if (IsProgress) 
                OnCompleted.Invoke();
        }

        // protected void ConsumeResource() => Provider.DynamicStatEntry.Resource.Value -= cost;
        protected void StartCooling() => coolTimeRoutine = StartCoroutine(CoolingRoutine());
        protected void StartProgression(Action callback = null)
        {
            progressRoutine = StartCoroutine(Progressing(callback));
        }
        
        protected void StopProgression()
        {
            if (progressRoutine != null) StopCoroutine(progressRoutine);
            
            ResetProgress();
        }

        protected virtual void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime);
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

        private IEnumerator Progressing(Action callback)
        {
            var endTime = progressTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            while (CastingProgress.Value < endTime)
            {
                CastingProgress.Value += Time.deltaTime;
                yield return null;
            }

            callback?.Invoke();
            ResetProgress();
        }

        private void ResetProgress()
        {
            progressRoutine       = null;
            CastingProgress.Value = 0f;
        }

        protected virtual void Awake()
        {
            Provider        = GetComponentInParent<ICombatProvider>();
            CharacterSystem = GetComponentInParent<ICharacterSystem>();
            
            OnActivated.Register("IsEndedToFalse", () => IsEnded = false);
            OnActivated.Register("PlayAnimation", PlayAnimation);
            
            OnCanceled.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("Idle", CharacterSystem.Animating.Idle);
            OnEnded.Register("IsProgressionToFalse", () => IsProgress = false);
            OnEnded.Register("IsEndedToTrue", () => IsEnded           = true);

            if (coolTime != 0f) ConditionTable.Register("IsCoolTimeReady", IsCoolTimeReady);
            if (cost != 0f ) ConditionTable.Register("IsCostReady", IsCostReady);
            if (progressTime != 0f)
            {
                OnActivated.Register("StartProgress", () => StartProgression(OnCompleted.Invoke));
                OnEnded.Register("StopProgress", StopProgression);
            }
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            var skillData = MainData.SkillSheetData(actionCode);

            skillType    = skillData.SkillType.ToEnum<SkillType>();
            priority     = skillData.BasePriority;
            range        = skillData.TargetParam.x;
            angle        = skillData.TargetParam.y;
            description  = skillData.Description;
            coolTime     = skillData.CoolTime;
            cost         = skillData.Cost;
            animationKey = skillData.AnimationKey;
            progressTime = skillData.ProcessTime;
            sortingType  = skillData.SortingType.ToEnum<SortingType>();
            targetLayer  = LayerMask.GetMask(skillData.TargetLayer);
            icon         = GetSkillIcon();
        }

        public void ShowDataBase()
        {
            var skillData = MainData.SheetDataTable[DataIndex.Skill];

            UnityEditor.EditorUtility.OpenPropertyEditor(skillData);
        } 
        
        private Sprite GetSkillIcon()
        {
            return !(MainData.TryGetIcon(actionCode.ToString(), out var result))
                ? null
                : result;
        }
#endif
    }
}
