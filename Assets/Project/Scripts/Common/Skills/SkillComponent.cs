using System.Collections.Generic;
using Common.Characters;
using Common.Execution;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillComponent : MonoBehaviour, IActionSender, IEditable
    // , ISequencer 
    {
        [SerializeField] protected Executor executor;
        [SerializeField] protected SkillSequencer sequencer;
        
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected SortingType sortingType;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected ActionMask ignorableMask = ActionMask.SkillIgnoreMask;
        [SerializeField] protected int priority;
        [SerializeField] protected float range;
        [SerializeField] protected float angle;
        [SerializeField] protected string animationKey;
        [SerializeField] protected string description;
        [SerializeField] protected CoolTimer coolTimer;
        [SerializeField] protected CastTimer castTimer;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected Sprite icon;
        
        private CharacterBehaviour cb;
        private bool isInitialized; // TODO. OnEnable, Disable 삭제 문제가 없어지면 같이 삭제

        public DataIndex DataIndex => actionCode;
        public ICombatProvider Provider => Cb;
        public SkillType SkillType => skillType;
        public ActionMask BehaviourMask => behaviourMask;
        public ActionMask IgnorableMask => ignorableMask;
        public int Priority => priority;
        public float Range => range;
        public float Angle => angle;
        public string Description => description;
        public LayerMask TargetLayer => targetLayer;
        public Sprite Icon => icon;
        
        // TODO. IProjectorSequence 작업 시 변경. range, angle을 sizeVector2로 치환 시도 해보자. 
        [SerializeField] protected Vector2 sizeVector;
        public Vector2 SizeVector => sizeVector;
        // 
        
        /* Sequence */
        public ConditionTable Condition => sequencer.Condition;
        public ActionTable ActiveAction => sequencer.ActiveAction;
        public ActionTable CancelAction => sequencer.CancelAction;
        public ActionTable CompleteAction => sequencer.CompleteAction;
        public ActionTable EndAction => sequencer.EndAction;
        public ActionTable ExecuteAction { get; } = new();
        
        public SkillSequenceBuilder SequenceBuilder { get; } = new();
        public SkillSequenceInvoker SequenceInvoker { get; } = new();

        /* Progress */
        public CoolTimer CoolTimer => coolTimer;
        public CastTimer CastTimer => castTimer;
        
        // TODO. Will be Multiply Character Haste Weight
        public float CoolWeightTime => CoolTimer.CoolTime;
        public float CastWeightTime => CastTimer.CastingTime;
        public virtual ICombatTaker MainTarget =>
            Cb.Searching.GetMainTarget(targetLayer, Cb.transform.position, sortingType);

        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public bool IsEnded => sequencer == null || SequenceInvoker.IsEnd;
        protected bool AbleToRelease => SkillType is not (SkillType.Instant or SkillType.Casting) && IsActive;
        protected bool IsActive => sequencer == null || SequenceInvoker.IsActive;


        /// <summary>
        /// 스킬 사용시 호출.
        /// </summary>
        public void Active(Vector3 targetPosition)
        {
            SequenceInvoker.Active(targetPosition);
        }

        /// <summary>
        /// 플레이어가 시전 중 이동하거나 cc를 받아 기술 취소 시 호출. 
        /// </summary>
        public void Cancel()
        {
            SequenceInvoker.Cancel();
        }

        /// <summary>
        /// 버튼을 띌 때 호출되며 차징, 홀딩 및 차징 스킬에 유효.
        /// </summary>
        public void Release()
        {
            if (!AbleToRelease) return;

            CastTimer.CallbackSection.GetInvokeAction(this)?.Invoke();
        }

        /// <summary>
        /// 스킬의 가동범위로 부터 대상을 받아서
        /// 데미지, 상태이상 부여 등을 실제 수행하는 함수
        /// </summary>
        public virtual void Execution() { } // => ExecuteAction.Invoke();
        
        public virtual void Initialize()
        {
            if (isInitialized) return;
            
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer);

            if (coolTimer.InvokeSection != SectionType.None)
            {
                SequenceBuilder.AddCondition("IsCoolTimeReady", () => coolTimer.IsReady)
                               .Add(coolTimer.InvokeSection, "ActiveCoolTime", () => coolTimer.Play(CoolWeightTime));
            }

            SequenceBuilder.AddActiveParam("CharacterRotate", Cb.Rotate)
                           .AddActive("PlayAnimation", PlayAnimation)
                           .AddActive("SkillCasting", 
                                      () => castTimer.Play(CastWeightTime, CastTimer.CallbackSection.GetInvokeAction(this)))
                           .AddActive("StopPathfinding", Cb.Pathfinding.Stop)
                           .AddEnd("StopCastTimer", castTimer.Stop)
                           .AddEnd("CharacterStop", Cb.Stop);
            
            AddSkillSequencer();
            isInitialized = true;
        }

        public virtual void Dispose()
        {
            sequencer.Clear();
            
            // SkillSequencer.Clear();
            coolTimer.Dispose();
            castTimer.Dispose();
        } 
        
        
        /// <summary>
        /// 스킬 별 각자의 시퀀스 적용 함수
        /// </summary>
        protected virtual void AddSkillSequencer() { }
        
        
        protected virtual void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, 0f, SequenceInvoker.Complete);
        }

        // Utility 
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
        
        protected void AddAnimationEvent()
        {
            SequenceBuilder.AddActive("RegisterHitEvent", () => Cb.Animating.OnHit.Add("SkillHit", Execution))
                           .AddEnd("ReleaseHit", () => Cb.Animating.OnHit.Remove("SkillHit"));
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            executor = GetComponentInChildren<Executor>();
            
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

            coolTimer.CoolTime = skillData.CoolTime;
            castTimer.CastingTime = skillData.ProcessTime;
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