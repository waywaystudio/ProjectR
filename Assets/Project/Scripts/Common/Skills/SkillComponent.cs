using System;
using System.Collections;
using System.Collections.Generic;
using Common.Characters;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillComponent : MonoBehaviour, ISequence, IDataIndexer, IEditable
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

        /* Animation */
        [SerializeField] protected string animationKey;

        private CharacterBehaviour cb;
        

        /* Sequence */
        public ConditionTable Conditions { get; } = new();
        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        public ActionTable<ICombatTaker> OnCompletion { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnEnded { get; } = new();

        public SkillType SkillType => skillType;
        public bool IsRigid => isRigid;
        public bool IsEnded { get; set; } = true;
        public int Priority => priority;
        public float Range => range;
        public float Angle => angle;

        public string Description => description;
        public FloatEvent CastingProgress { get; } = new(0, float.MaxValue);
        public LayerMask TargetLayer => targetLayer;
        public Sprite Icon => icon;
        public DataIndex ActionCode => actionCode;
        public virtual ICombatTaker MainTarget =>
            Cb.Searching.GetMainTarget(targetLayer, Cb.transform.position, sortingType);
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        
        protected bool AbleToRelease => SkillType is not (SkillType.Instant or SkillType.Casting) && IsProgress;
        protected bool IsProgress { get; set; }

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
        /// 대상(ICombatTaker)을 찾아 Completion을 실행하는 함수
        /// </summary>
        public abstract void MainAttack();


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
        /// 스킬 시퀀스 중에서 전투값을 대상에게 실제로 전달하는 함수.
        /// 충돌 + 데미지 종류 + 상태이상 등이 있을 수 있다.
        /// OnCompletion 하위 컴포넌트 중 Completion에서 실제 구현된다.
        /// TODO. Complete와 이름이 비슷하여 안좋다. 바꾸자. 
        /// </summary>
        protected void Completion(ICombatTaker taker)
        {
            OnCompletion.Invoke(taker);
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
        protected void Dispose()
        {
            Conditions.Clear();
            OnActivated.Clear();
            OnCanceled.Clear();
            OnCompletion.Clear();
            OnCompleted.Clear();
            OnEnded.Clear();
        }
        
        
        /* Progress Entity */
        [SerializeField] protected float progressTime;
        private Coroutine progressRoutine;
        public float ProgressTime => progressTime;

        protected void StartProgression(Action callback = null)
        {
            progressRoutine = StartCoroutine(Casting(callback));
        }
        
        protected void StopProgression()
        {
            if (progressRoutine != null) StopCoroutine(progressRoutine);
            
            ResetProgress();
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
        

        protected virtual void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, progressTime, Complete);
        }

        protected bool TryGetTakersInSphere(SkillComponent skill, out List<ICombatTaker> takerList) => (takerList = 
                Cb.Colliding.GetTakersInSphereType(
                skill.Cb.transform.position, 
                skill.Range, 
                skill.Angle, 
                skill.TargetLayer)
            ).HasElement();

        protected virtual void Awake()
        {
            // TODO. Cost, CoolTime, Casting을 Component화 하면 빠질 수 있다.
            if (progressTime != 0f)
            {
                OnActivated.Register("StartProgress", () => StartProgression(Complete));
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
            animationKey = skillData.AnimationKey;
            progressTime = skillData.ProcessTime;
            sortingType  = skillData.SortingType.ToEnum<SortingType>();
            targetLayer  = LayerMask.GetMask(skillData.TargetLayer);
            icon         = GetSkillIcon();

            if (TryGetComponent(out SkillCoolTime coolTimeModule))
            {
                coolTimeModule.EditorSetUp();
            }
        }
        private void ShowDataBase()
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

// [SerializeField] private float cost;
// public float Cost => cost;
// protected void ConsumeResource() => Provider.DynamicStatEntry.Resource.Value -= cost;
// protected bool IsCostReady()
// {
//     return Cb.DynamicStatEntry.Resource.Value >= cost;
// }