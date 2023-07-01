using Common.Characters;
using Common.Execution;
using Common.Systems;
using UnityEngine;

namespace Common.Skills
{
    public class SkillComponent : MonoBehaviour, IActionSender, IActionBehaviour, IHasSequencer, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected int priority;
        [SerializeField] protected Executor executor;
        [SerializeField] protected Detector detector;
        [SerializeField] protected SkillAnimationTrait animationTrait;
        [SerializeField] protected SkillCoolTimer coolTimer;
        [SerializeField] protected SkillCastTimer castTimer;
        [SerializeField] protected SkillCost cost;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string description;
        
        private CharacterBehaviour cb;
        private readonly SkillSequencer sequencer = new();

        public DataIndex DataIndex => actionCode;
        public ICombatProvider Provider => Cb;
        public ActionMask BehaviourMask => behaviourMask;
        public ICombatTaker MainTarget => detector?.GetMainTarget();
        public int Priority => priority;
        public SkillCost Cost => cost;
        public string Description => description;
        public float Haste => Cb.StatTable.Haste;
        public float Range => detector.Range;
        public float Angle => detector.Angle;
        public Sprite Icon => icon;
        public Sequencer Sequencer { get; } = new();
        public SkillSequenceBuilder SequenceBuilder { get; private set; }
        public SkillSequenceInvoker SkillInvoker { get; private set; }
        public SkillCoolTimer CoolTimer => coolTimer;
        public SkillCastTimer CastTimer => castTimer;

        // TODO. Will be Multiply Character Haste Weight
        public float CoolWeightTime => CoolTimer.CoolTime;
        public float CastWeightTime => CastTimer.CastingTime;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public bool IsEnded => Sequencer == null || SkillInvoker.IsEnd;
        public bool AbleToRelease => animationTrait.SkillType is not (SkillType.Instant or SkillType.Casting) && IsActive;
        protected bool IsActive => Sequencer == null || SkillInvoker.IsActive;


        public virtual void Initialize()
        {
            SkillInvoker    = new SkillSequenceInvoker(sequencer);
            SequenceBuilder = new SkillSequenceBuilder(sequencer);
            
            detector.Initialize(Cb);
            animationTrait.Initialize(this);
            coolTimer.Initialize(this);
            castTimer.Initialize(this);
            cost.Initialize(this);
            
            SequenceBuilder.Add(SectionType.Active, "StopPathfinding", Cb.Pathfinding.Stop)
                           .Add(SectionType.End,"CharacterStop", Cb.Stop)
                           .Add(SectionType.Release, "ReleaseAction", () =>
                           {
                               if (AbleToRelease) 
                                   CastTimer.CallbackSection.GetInvokeAction(this)?.Invoke();
                           });
        }

        public void Cancel() => SkillInvoker.Cancel();

        public virtual void Dispose()
        {
            SkillInvoker.End();
            
            sequencer.Clear();
            coolTimer.Dispose();
            castTimer.Dispose();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            executor.EditorGetExecutions(gameObject);
            detector.SetUpAsSkill(actionCode);
            coolTimer.SetUpAsSkill(actionCode);
            castTimer.SetUpFromSkill(actionCode);
            cost.SetUpFromSkill(actionCode);
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