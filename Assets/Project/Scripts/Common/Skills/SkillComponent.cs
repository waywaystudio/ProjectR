using System.Collections.Generic;
using Common.Characters;
using Common.Execution;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillComponent : MonoBehaviour, IActionSender, IHasSequencer, IEditable
    {
        [SerializeField] protected Executor executor;
        [SerializeField] protected Sequencer<Vector3> sequencer;
        [SerializeField] protected SkillAnimationTrait animationTrait;
        [SerializeField] protected CoolTimer coolTimer;
        [SerializeField] protected CastTimer castTimer;
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected SortingType sortingType;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected int priority;
        [SerializeField] protected float range;
        [SerializeField] protected float angle;
        [SerializeField] protected string description;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected Sprite icon;
        
        private CharacterBehaviour cb;

        public DataIndex DataIndex => actionCode;
        public ICombatProvider Provider => Cb;
        public ActionMask BehaviourMask => behaviourMask;
        public int Priority => priority;
        public float Range => range;
        public float Angle => angle;
        public string Description => description;
        public LayerMask TargetLayer => targetLayer;
        public Sprite Icon => icon;
        
        public Sequencer Sequencer => sequencer;
        public SequenceBuilder<Vector3> SequenceBuilder { get; private set; }
        public SkillSequenceInvoker SkillInvoker { get; private set; }
        public CoolTimer CoolTimer => coolTimer;
        public CastTimer CastTimer => castTimer;
        
        // TODO. IProjectorSequence 작업 시 변경. range, angle을 sizeVector2로 치환 시도 해보자. 
        [SerializeField] protected Vector2 sizeVector;
        public Vector2 SizeVector => sizeVector;
        
        // TODO. Will be Multiply Character Haste Weight
        public float CoolWeightTime => CoolTimer.CoolTime;
        public float CastWeightTime => CastTimer.CastingTime;
        public virtual ICombatTaker MainTarget =>
            Cb.Searching.GetMainTarget(targetLayer, Cb.transform.position, sortingType);

        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public bool IsEnded => sequencer == null || SkillInvoker.IsEnd;
        protected bool AbleToRelease => animationTrait.SkillType is not (SkillType.Instant or SkillType.Casting) && IsActive;
        protected bool IsActive => sequencer == null || SkillInvoker.IsActive;


        public virtual void Initialize()
        {
            SkillInvoker    = new SkillSequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder<Vector3>(sequencer);

            animationTrait.Initialize(this);

            if (coolTimer.InvokeSection != SectionType.None)
            {
                SequenceBuilder.AddCondition("IsCoolTimeReady", () => coolTimer.IsReady)
                               .Add(coolTimer.InvokeSection, "ActiveCoolTime", () => coolTimer.Play(CoolWeightTime));
            }

            SequenceBuilder.AddActiveParam("CharacterRotate", Cb.Rotate)
                           .Add(SectionType.Active,"SkillCasting",
                                () => castTimer.Play(CastWeightTime, CastTimer.CallbackSection.GetInvokeAction(this)))
                           .Add(SectionType.Active, "StopPathfinding", Cb.Pathfinding.Stop)
                           .Add(SectionType.End,"StopCastTimer", castTimer.Stop)
                           .Add(SectionType.End,"CharacterStop", Cb.Stop)
                           .Add(SectionType.Release, "ReleaseAction", () =>
                           {
                               if (AbleToRelease) CastTimer.CallbackSection.GetInvokeAction(this)?.Invoke();
                           });
                
        }

        public void Dispose()
        {
            sequencer.Clear();
            coolTimer.Dispose();
            castTimer.Dispose();
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


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            executor.EditorGetExecutions(gameObject);
            
            var skillData = Database.SkillSheetData(actionCode);

            animationTrait.SkillType = skillData.SkillType.ToEnum<SkillType>();
            animationTrait.Key       = skillData.AnimationKey;
            priority                 = skillData.BasePriority;
            range                    = skillData.TargetParam.x;
            angle                    = skillData.TargetParam.y;
            description              = skillData.Description;
            sortingType              = skillData.SortingType.ToEnum<SortingType>();
            targetLayer              = LayerMask.GetMask(skillData.TargetLayer);
            icon                     = Database.SpellSpriteData.Get(actionCode);
            coolTimer.CoolTime       = skillData.CoolTime;
            castTimer.CastingTime    = skillData.ProcessTime;
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