using System.Collections.Generic;
using Common.Characters;
using Common.Execution;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillComponent : MonoBehaviour, ISequencer, IActionSender, IEditable
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
        private readonly Sequencer<Vector3> mainSequencer = new();
        private readonly SequenceBuilder<Vector3> builder = new();


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
        public Sequencer Sequencer => sequencer;
        public SkillSequencer SkillSequencer => sequencer;
        
        // public ActionTable<Vector3> ActiveParamAction => SkillSequencer.ActiveParamAction;
        public ConditionTable Condition => SkillSequencer.Condition;
        public ActionTable ActiveAction => SkillSequencer.ActiveAction;
        public ActionTable CancelAction => SkillSequencer.CancelAction;
        public ActionTable CompleteAction => SkillSequencer.CompleteAction;
        public ActionTable EndAction => SkillSequencer.EndAction;
        
        public ActionTable ExecuteAction { get; } = new();

        /* Progress */
        public CoolTimer CoolTimer => coolTimer;
        public CastTimer CastTimer => castTimer;
        
        // TODO. Will be Multiply Character Haste Weight
        public float CoolWeightTime => CoolTimer.CoolTime;
        public float CastWeightTime => CastTimer.CastingTime;
        public virtual ICombatTaker MainTarget =>
            Cb.Searching.GetMainTarget(targetLayer, Cb.transform.position, sortingType);

        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public bool IsEnded => sequencer == null || sequencer.IsEnd;
        protected bool AbleToRelease => SkillType is not (SkillType.Instant or SkillType.Casting) && IsActive;
        protected bool IsActive => sequencer == null || sequencer.IsActive;


        /// <summary>
        /// 스킬 사용시 호출.
        /// </summary>
        public void Active(Vector3 targetPosition)
        {
            sequencer.Active(targetPosition);
        }

        /// <summary>
        /// 플레이어가 시전 중 이동하거나 cc를 받아 기술 취소 시 호출. 
        /// </summary>
        public void Cancel()
        {
            SkillSequencer.Cancel();
        }

        /// <summary>
        /// 버튼을 띌 때 호출되며 차징, 홀딩 및 차징 스킬에 유효.
        /// </summary>
        public void Release()
        {
            if (!AbleToRelease) return;

            var callbackSection = CastTimer.CallbackSection.GetSkillAction(this);

            callbackSection?.Invoke();
        }

        /// <summary>
        /// 스킬의 가동범위로 부터 대상을 받아서
        /// 데미지, 상태이상 부여 등을 실제 수행하는 함수
        /// </summary>
        public virtual void Execution() { } // => ExecuteAction.Invoke();
        
        /// <summary>
        /// 스킬 별 각자의 시퀀스 적용 함수
        /// </summary>
        protected virtual void AddSkillSequencer() { }
        
        
        protected virtual void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, 0f, SkillSequencer.Complete);
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
            SkillSequencer.ActiveAction
                     .Add("RegisterHitEvent", () => Cb.Animating.OnHit.Add("SkillHit", Execution));
            
            SkillSequencer.EndAction
                     .Add("ReleaseHit", () => Cb.Animating.OnHit.Remove("SkillHit"));
        }
        //

        private void Awake()
        {
            builder.Initialize(mainSequencer);
        }

        protected void OnEnable()
        {
            AddSkillSequencer();
            
            // builder.AddActive("PlaySkillAnimation", PlayAnimation)
            //        .AddActive("AddCastTimerCallback", () => 
            //        { 
            //            var callbackSection = CastTimer.CallbackSection.GetSkillAction(this);
            //            castTimer.Play(CastWeightTime, callbackSection);
            //        })
            //        .AddActive("StopPathfinding", Cb.Pathfinding.Stop)
            //        .AddEnd("StopCastTimer", castTimer.Stop)
            //        .AddEnd("MoveStop", Cb.Stop)
            //        .Build();
            
            if (coolTimer.InvokeSection != SectionType.None)
            {
                SkillSequencer.Condition.Add("IsCoolTimeReady", () => coolTimer.IsReady);

                var targetSection = coolTimer.InvokeSection switch
                {
                    SectionType.Active => SkillSequencer.ActiveAction,
                    SectionType.Complete   => SkillSequencer.CompleteAction,
                    SectionType.End        => SkillSequencer.EndAction,
                    SectionType.Execute    => ExecuteAction,
                    _                      => null,
                };
                
                targetSection?.Add("ActiveCoolTime", () => coolTimer.Play(CoolWeightTime));
            }

            SkillSequencer.ActiveParamAction.Add("CharacterRotate", Cb.Rotate);
            SkillSequencer.ActiveAction.Add("SkillCommonAction", () =>
            {
                PlayAnimation();

                var callbackSection = CastTimer.CallbackSection.GetSkillAction(this);
                castTimer.Play(CastWeightTime, callbackSection);
                Cb.Pathfinding.Stop();
            });
            
            SkillSequencer.EndAction.Add("SkillEndAction", () =>
            {
                castTimer.Stop();
                Cb.Stop();
            });
        }

        protected void OnDisable()
        {
            SkillSequencer.Clear();
            coolTimer.Dispose();
            castTimer.Dispose();
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