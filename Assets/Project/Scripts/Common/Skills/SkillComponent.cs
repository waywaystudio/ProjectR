using Common.Characters;
using Common.Execution;
using Common.Systems;
using UnityEngine;

namespace Common.Skills
{
    public abstract class SkillComponent : MonoBehaviour, IActionSender, IHasSequencer, IEditable
    {
        [SerializeField] protected Executor executor;
        [SerializeField] protected Detector detector;
        [SerializeField] protected Sequencer<Vector3> sequencer;
        [SerializeField] protected SkillAnimationTrait animationTrait;
        [SerializeField] protected CoolTimer coolTimer;
        [SerializeField] protected CastTimer castTimer;
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected int priority;
        [SerializeField] protected string description;
        [SerializeField] protected Sprite icon;
        
        private CharacterBehaviour cb;

        public DataIndex DataIndex => actionCode;
        public ICombatProvider Provider => Cb;
        public ActionMask BehaviourMask => behaviourMask;
        public int Priority => priority;
        public string Description => description;
        public Sprite Icon => icon;
        public float Range => detector.Range;
        public ICombatTaker MainTarget => detector.GetMainTarget();
        
        public Sequencer Sequencer => sequencer;
        public SequenceBuilder<Vector3> SequenceBuilder { get; private set; }
        public SkillSequenceInvoker SkillInvoker { get; private set; }
        public CoolTimer CoolTimer => coolTimer;
        public CastTimer CastTimer => castTimer;

        // TODO. Will be Multiply Character Haste Weight
        public float CoolWeightTime => CoolTimer.CoolTime;
        public float CastWeightTime => CastTimer.CastingTime;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public bool IsEnded => sequencer == null || SkillInvoker.IsEnd;
        protected bool AbleToRelease => animationTrait.SkillType is not (SkillType.Instant or SkillType.Casting) && IsActive;
        protected bool IsActive => sequencer == null || SkillInvoker.IsActive;


        public virtual void Initialize()
        {
            SkillInvoker    = new SkillSequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder<Vector3>(sequencer);

            detector.Initialize(Cb);
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


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            executor.EditorGetExecutions(gameObject);
            detector.SetUpAsSkill(actionCode);
            
            var skillData = Database.SkillSheetData(actionCode);

            animationTrait.SkillType = skillData.SkillType.ToEnum<SkillType>();
            animationTrait.Key       = skillData.AnimationKey;
            priority                 = skillData.BasePriority;
            description              = skillData.Description;
            coolTimer.CoolTime       = skillData.CoolTime;
            castTimer.CastingTime    = skillData.ProcessTime;
            icon                     = Database.SpellSpriteData.Get(actionCode);
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