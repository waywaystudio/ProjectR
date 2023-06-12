using System.Collections.Generic;
using Common.Characters;
using Common.Execution;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillComponent : MonoBehaviour, IOldConditionalSequence, IExecutable, IDataIndexer, IEditable
    {
        /* Common Attribution */
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected SortingType sortingType;
        [SerializeField] protected CharacterActionMask behaviourMask = CharacterActionMask.Skill;
        [SerializeField] protected CharacterActionMask ignorableMask = CharacterActionMask.SkillIgnoreMask;
        [SerializeField] protected int priority;
        [SerializeField] protected float range;
        [SerializeField] protected float angle;
        [SerializeField] protected string animationKey;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        

        private CharacterBehaviour cb;

        /* Sequence */
        public ConditionTable Conditions { get; } = new();
        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnCanceled { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();
        [ShowInInspector] public bool IsEnded { get; set; } = true;
        
        public ExecutionTable ExecutionTable { get; } = new();

        public DataIndex DataIndex => actionCode;
        public ICombatProvider Provider => Cb;
        public SkillType SkillType => skillType;
        public CharacterActionMask BehaviourMask => behaviourMask;
        public CharacterActionMask IgnorableMask => ignorableMask;
        public int Priority => priority;
        public float Range => range;
        public float Angle => angle;
        public string Description => description;
        public float CastingTime { get; set; }
        public LayerMask TargetLayer => targetLayer;
        public Sprite Icon => icon;
        public virtual ICombatTaker MainTarget =>
            Cb.Searching.GetMainTarget(targetLayer, Cb.transform.position, sortingType);
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        
        protected bool AbleToRelease => SkillType is not (SkillType.Instant or SkillType.Casting) && IsProgress;
        protected bool IsProgress { get; set; }

        protected abstract void Initialize();
        

        /// <summary>
        /// 스킬 사용시 호출.
        /// </summary>
        public void Activate(Vector3 targetPosition)
        {
            IsProgress = true;
            IsEnded    = false;
            
            PlayAnimation();
            
            Cb.Rotate(targetPosition);
            Cb.Pathfinding.Stop();
         
            OnActivated.Invoke();
        }

        /// <summary>
        /// 스킬의 가동범위로 부터 대상을 받아서
        /// 데미지, 상태이상 부여 등을 실제 수행하는 함수
        /// </summary>
        public abstract void Execution();

        /// <summary>
        /// 플레이어가 시전 중 이동하거나 cc를 받아 기술 취소 시 호출. 
        /// </summary>
        public void Cancel()
        {
            IsProgress = false;

            OnCanceled.Invoke();
            End();
        }

        /// <summary>
        /// 버튼을 띌 때 호출되며 차징, 홀딩 스킬에 유효.
        /// </summary>
        public void Release()
        {
            AbleToRelease.OnTrue(Complete);
        }

        /// <summary>
        /// 기술을 성공적으로 만료시 호출
        /// </summary>
        public void Complete()
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
            OnCompleted.Clear();
            OnEnded.Clear();
            ExecutionTable.Clear();
        }

        protected virtual void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, 0f, Complete);
        }

        protected bool TryGetTakersInSphere(SkillComponent skill, out List<ICombatTaker> takerList) 
            => (takerList =  Cb.Colliding.GetTakersInSphereType(
                skill.Cb.transform.position,
                skill.Range, 
                skill.Angle, 
                skill.TargetLayer)).HasElement();

        protected bool TryGetTakersByRayCast(out List<ICombatTaker> takerList)
        {
            var providerTransform = Cb.transform;

            return Cb.Colliding.TryGetTakersByRaycast(
                providerTransform.position, 
                providerTransform.forward, range, 16,
                targetLayer, out takerList);
        }

        protected void OnEnable() => Initialize();
        protected void OnDisable() => Dispose();


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            var skillData = Database.SkillSheetData(actionCode);

            skillType    = skillData.SkillType.ToEnum<SkillType>();
            priority     = skillData.BasePriority;
            range        = skillData.TargetParam.x;
            angle        = skillData.TargetParam.y;
            description  = skillData.Description;
            animationKey = skillData.AnimationKey;
            sortingType  = skillData.SortingType.ToEnum<SortingType>();
            targetLayer  = LayerMask.GetMask(skillData.TargetLayer);
            icon         = Database.SpellSpriteData.Get(actionCode);

            if (TryGetComponent(out CoolTimer coolTimeModule))
            {
                coolTimeModule.EditorSetUp();
            }
        }
        
        // ReSharper disable once UnusedMember.Local
        private void ShowDataBase()
        {
            var skillData = Database.SheetDataTable[DataIndex.Skill];

            UnityEditor.EditorUtility.OpenPropertyEditor(skillData);
        }
#endif
    }
}