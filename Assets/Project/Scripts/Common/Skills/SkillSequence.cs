using System;
using System.Collections;
using System.Collections.Generic;
using Common.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillSequence : MonoBehaviour, ISequence, IDataIndexer, IEditable
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

        private CharacterBehaviour cb;
        private Coroutine progressRoutine;
        private Coroutine coolTimeRoutine;

        /* Sequence */
        [ShowInInspector] public ConditionTable Conditions { get; } = new();
        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnHit { get; } = new();
        [ShowInInspector] public ActionTable OnCanceled { get; } = new();
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
        public virtual ICombatTaker MainTarget =>
            Cb.Searching.GetMainTarget(targetLayer, Cb.transform.position, sortingType);
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        
        protected bool IsProgress { get; set; }
        protected bool AbleToRelease => SkillType is not (SkillType.Instant or SkillType.Casting) && IsProgress;

        protected abstract void Initialize();


        /// <summary>
        /// 스킬 사용시 호출.
        /// </summary>
        public void Execution(Vector3 targetPosition)
        {
            IsProgress = true;
            IsEnded    = false;
            
            PlayAnimation();
            
            Cb.Rotate(targetPosition);
            Cb.Pathfinding.Stop();
            
            OnActivated.Invoke();
        }

        
        /// <summary>
        /// 스킬 시퀀스 중에서 전투값을 대상에게 실제로 전달하는 함수.
        /// 충돌 + 데미지 종류 + 상태이상 등이 있을 수 있다.
        /// 스킬마다 호출되는 방법이 다르지만, 반드시 호출하는 곳이 있다.
        /// </summary>
        public virtual void OnAttack() { }

        
        /// <summary>
        /// 플레이어가 시전 중 이동하거나 cc를 받아 기술 취소 시 호출. 
        /// </summary>
        public void Cancellation()
        {
            IsProgress = false;
            
            OnCanceled.Invoke();
            End();
        }

        
        /// <summary>
        /// 버튼을 띌 때 호출되며 차징형 스킬에만 유효.
        /// </summary>
        public void Release()
        {
            AbleToRelease.OnTrue(Complete);
        }


        /// <summary>
        /// 기술을 성공적으로 만료시 호출
        /// </summary>
        protected void Complete()
        {
            IsProgress = false;
            
            OnCompleted.Invoke();
        }

        
        /// <summary>
        /// 만료시 호출 (성공 실패 무관)
        /// </summary>
        protected void End()
        {
            IsEnded = true;
                
            Cb.Stop();
            OnEnded.Invoke();
        }
        
        
        /// <summary>
        /// 씬 종료 혹은 SkillSequence GameObject가 꺼질 때 호출.
        /// </summary>
        protected virtual void Dispose()
        {
            this.Clear();
        }


        // protected void ConsumeResource() => Provider.DynamicStatEntry.Resource.Value -= cost;
        protected void StartCooling() => coolTimeRoutine = StartCoroutine(Cooling());
        protected void StartProgression(Action callback = null)
        {
            progressRoutine = StartCoroutine(Casting(callback));
        }
        
        protected void StopProgression()
        {
            if (progressRoutine != null) StopCoroutine(progressRoutine);
            
            ResetProgress();
        }

        protected virtual void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, progressTime, Complete);
        }
        
        protected bool IsCoolTimeReady()
        {
            return RemainCoolTime.Value <= 0.0f;
        }

        protected bool IsCostReady()
        {
            return Cb.DynamicStatEntry.Resource.Value >= cost;
        }
        
        protected bool TryGetTakersInSphere(SkillSequence skill, out List<ICombatTaker> takerList) => (takerList = 
                Cb.Colliding.GetTakersInSphereType(
                skill.Cb.transform.position, 
                skill.Range, 
                skill.Angle, 
                skill.TargetLayer)
            ).HasElement();

        private IEnumerator Cooling()
        {
            RemainCoolTime.Value = coolTime;

            while (RemainCoolTime.Value > 0f)
            {
                RemainCoolTime.Value -= Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Casting(Action callback)
        {
            var endTime = progressTime * CharacterUtility.GetHasteValue(Cb.StatTable.Haste);

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
            // TODO. Cost, CoolTime, Casting을 Component화 하면 빠질 수 있다.
            if (coolTime != 0f) Conditions.Register("IsCoolTimeReady", IsCoolTimeReady);
            if (cost     != 0f ) Conditions.Register("IsCostReady", IsCostReady);
            if (progressTime != 0f)
            {
                OnActivated.Register("StartProgress", () => StartProgression(OnCompleted.Invoke));
                OnEnded.Register("StopProgress", StopProgression);
            }
        }

        protected void OnEnable() => Initialize();
        protected void OnDisable() => Dispose();


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            var skillData = Database.SkillSheetData(actionCode);

            skillType    = skillData.SkillType.ToEnum<SkillType>();
            isRigid      = skillData.IsRigid;
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
            var skillData = Database.SheetDataTable[DataIndex.Skill];

            UnityEditor.EditorUtility.OpenPropertyEditor(skillData);
        } 
        
        private Sprite GetSkillIcon()
        {
            return !(Database.TryGetIcon(actionCode.ToString(), out var result))
                ? null
                : result;
        }
#endif
    }
}
