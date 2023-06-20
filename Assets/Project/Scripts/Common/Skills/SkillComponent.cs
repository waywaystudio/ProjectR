using Common.Characters;
using Common.Execution;
using Common.Systems;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillComponent : MonoBehaviour, IActionSender, IActionBehaviour, IHasSequencer, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected int priority;
        [SerializeField] protected Executor executor;
        [SerializeField] protected Detector detector;
        [SerializeField] protected Sequencer<Vector3> sequencer;
        [SerializeField] protected SkillAnimationTrait animationTrait;
        [SerializeField] protected SkillCoolTimer coolTimer;
        [SerializeField] protected SkillCastTimer castTimer;
        [SerializeField] protected string description;
        [SerializeField] protected Sprite icon;
        
        private CharacterBehaviour cb;

        public DataIndex DataIndex => actionCode;
        public ICombatProvider Provider => Cb;
        public ActionMask BehaviourMask => behaviourMask;
        public ICombatTaker MainTarget => detector.GetMainTarget();
        public int Priority => priority;
        public string Description => description;
        public float Range => detector.Range;
        public float Angle => detector.Angle;
        public Sprite Icon => icon;
        public Sequencer Sequencer => sequencer;
        public SequenceBuilder<Vector3> SequenceBuilder { get; private set; }
        public SkillSequenceInvoker SkillInvoker { get; private set; }
        public SkillCoolTimer CoolTimer => coolTimer;
        public SkillCastTimer CastTimer => castTimer;

        // TODO. Will be Multiply Character Haste Weight
        public float CoolWeightTime => CoolTimer.CoolTime;
        public float CastWeightTime => CastTimer.CastingTime;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public bool IsEnded => sequencer == null || SkillInvoker.IsEnd;
        public bool AbleToRelease => animationTrait.SkillType is not (SkillType.Instant or SkillType.Casting) && IsActive;
        protected bool IsActive => sequencer == null || SkillInvoker.IsActive;


        public virtual void Initialize()
        {
            SkillInvoker    = new SkillSequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder<Vector3>(sequencer);

            detector.Initialize(Cb);
            animationTrait.Initialize(this);
            coolTimer.Initialize(this);
            castTimer.Initialize(this);

            SequenceBuilder.AddActiveParam("CharacterRotate", Cb.Rotate)
                           .Add(SectionType.Active, "StopPathfinding", Cb.Pathfinding.Stop)
                           .Add(SectionType.End,"CharacterStop", Cb.Stop)
                           .Add(SectionType.Release, "ReleaseAction", () =>
                           {
                               if (AbleToRelease && SkillInvoker.IsAbleToActive) 
                                   CastTimer.CallbackSection.GetInvokeAction(this)?.Invoke();
                           });
        }

        public void Cancel() => SkillInvoker.Cancel();

        public virtual void Dispose()
        {
            sequencer.Clear();
            coolTimer.Dispose();
            castTimer.Dispose();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            executor.EditorGetExecutions(gameObject);
            detector.SetUpAsSkill(actionCode);
            coolTimer.SetUpAsSkill(actionCode);
            castTimer.SetUpFromSkill(actionCode);
            animationTrait.SetUpFromSkill(actionCode);
            
            var skillData = Database.SkillSheetData(actionCode);

            behaviourMask = skillData.BehaviourMask.ToEnum<ActionMask>();
            priority      = skillData.Priority;
            description   = skillData.Description;
            icon          = Database.SpellSpriteData.Get(actionCode);
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