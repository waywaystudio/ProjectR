using System.Collections.Generic;
using Common.Characters;
using Common.Execution;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillComponent : MonoBehaviour, IExecutable, ISequencer<Vector3>, IDataIndexer, IEditable
    {
        /* Common Attribution */
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected SortingType sortingType;
        [SerializeField] protected SectionType coolTimeSection;
        [SerializeField] protected CharacterActionMask behaviourMask = CharacterActionMask.Skill;
        [SerializeField] protected CharacterActionMask ignorableMask = CharacterActionMask.SkillIgnoreMask;
        [SerializeField] protected int priority;
        [SerializeField] protected float range;
        [SerializeField] protected float angle;
        [SerializeField] protected string animationKey;
        [SerializeField] private string description;
        [SerializeField] protected AwaitCoolTimer coolTimer;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] private Sprite icon;
        
        public DataIndex DataIndex => actionCode;
        public ICombatProvider Provider => Cb;
        public SkillType SkillType => skillType;
        public CharacterActionMask BehaviourMask => behaviourMask;
        public CharacterActionMask IgnorableMask => ignorableMask;
        public int Priority => priority;
        public float Range => range;
        public float Angle => angle;
        public string Description => description;
        public LayerMask TargetLayer => targetLayer;
        public Sprite Icon => icon;
        
        
        /* Sequence */
        [SerializeField] private SkillSequencer sequencer;
        public Sequencer<Vector3> Sequencer => sequencer;

        public ConditionTable Condition => Sequencer.Condition;
        public ActionTable<Vector3> ActiveParamAction => Sequencer.ActiveParamAction;
        public ActionTable ActiveAction => Sequencer.ActiveAction;
        public ActionTable CancelAction => Sequencer.CancelAction;
        public ActionTable CompleteAction => Sequencer.CompleteAction;
        public ActionTable EndAction => Sequencer.EndAction;
        
        [ShowInInspector] public bool IsEnded { get; set; } = true;
        

        /* Execution */
        [SerializeField] protected SkillExecutor executor;
        
        public void Execute(ICombatTaker taker) => executor.Execute(taker);
        public void Execute(ExecuteGroup group, ICombatTaker taker) => executor.Execute(group, taker);
        public void Execute(Vector3 position) => executor.Execute(position);
        public void Execute(ExecuteGroup group, Vector3 position) => executor.Execute(group, position);
        public void AddExecution(ExecuteComponent exe) => executor.Add(exe);
        public void Remove(ExecuteComponent exe) => executor.Remove(exe);
        public void Clear() => executor.Clear();

        public ExecutionTable ExecutionTable { get; } = new();

        /* Progress */
        public AwaitCoolTimer CoolTimer => coolTimer;
        
        public float CastingTime { get; set; }
        public virtual ICombatTaker MainTarget =>
            Cb.Searching.GetMainTarget(targetLayer, Cb.transform.position, sortingType);

        private CharacterBehaviour cb;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        
        
        protected bool AbleToRelease => SkillType is not (SkillType.Instant or SkillType.Casting) && IsProgress;
        protected bool IsProgress 
            // => !ActiveParamSection.IsDone; 
            { get; set; }

        protected abstract void Initialize();
        

        /// <summary>
        /// 스킬 사용시 호출.
        /// </summary>
        public void Activate(Vector3 targetPosition)
        {
            sequencer.Active(targetPosition);
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
            Sequencer.Cancel();
        }

        /// <summary>
        /// 버튼을 띌 때 호출되며 차징, 홀딩 및 차징 스킬에 유효.
        /// </summary>
        public void Release()
        {
            AbleToRelease.OnTrue(Sequencer.CompleteAction.Invoke);
        }
        

        protected virtual void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, 0f, Sequencer.Complete);
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


        protected void OnEnable()
        {
            Initialize();
            coolTimer.Register(sequencer, coolTimeSection);
            
            Sequencer.Condition.Add("IsCoolTimeReady", () => coolTimer.IsReady);
            Sequencer.ActiveParamAction.Add("CharacterRotate", Cb.Rotate);
            Sequencer.ActiveAction.Add("SkillCommonAction", () =>
            {
                Cb.Pathfinding.Stop();
                PlayAnimation();
                IsProgress = true;
                IsEnded    = false;
            });
            Sequencer.EndAction.Add("SkillEndAction", () =>
            {
                IsProgress = false;
                IsEnded    = true;
                Cb.Stop();
            });
        }

        protected void OnDisable()
        {
            Sequencer.Clear();
            ExecutionTable.Clear();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            executor = GetComponentInChildren<SkillExecutor>();
            
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

            coolTimer.Timer = skillData.CoolTime;
        }
        
        // ReSharper disable once UnusedMember.Local
        private void EditorOpenDataBase()
        {
            var skillData = Database.SheetDataTable[DataIndex.Skill];

            UnityEditor.EditorUtility.OpenPropertyEditor(skillData);
        }
#endif
    }
}