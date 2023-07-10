using Common.Characters;
using Common.Effects;
using Common.Execution;
using Common.TargetSystem;
using UnityEngine;

namespace Common.Skills
{
    public class SkillComponent : MonoBehaviour, ISkill, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected int priority;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected Effector effector; 
        [SerializeField] protected CombatTakerDetector detector;
        [SerializeField] protected SkillAnimationTrait animationTrait;
        [SerializeField] protected SkillCoolTimer coolTimer;
        [SerializeField] protected SkillCastTimer castTimer;
        [SerializeField] protected SkillCost cost;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string description;
        
        private CharacterBehaviour cb;

        public DataIndex DataIndex => actionCode;
        public ActionMask BehaviourMask => behaviourMask;
        public ICombatProvider Provider => Cb;
        public ICombatTaker MainTarget => detector?.GetMainTarget();
        public ICombatTaker Taker { get; protected set; }
        public int Priority => priority;
        public SkillCost Cost => cost;
        public string Description => description;
        public float Haste => Cb.StatTable.Haste;
        public float Distance => detector.Distance;
        public float Range => detector.Range;
        public float Angle => detector.Angle;
        public Sprite Icon => icon;

        public CombatSequence Sequence { get; } = new();
        public CombatSequenceBuilder Builder { get; private set; }
        public CombatSequenceInvoker Invoker { get; private set; }
        public SkillCoolTimer CoolTimer => coolTimer;
        public SkillCastTimer CastTimer => castTimer;
        public Vector3 SizeVector => detector.SizeVector;
        public float CastingWeight => CastTimer.CastingTime;
        public float CoolingWeight => CoolTimer.CoolTime;
        
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public bool IsEnded => Sequence == null || Invoker.IsEnd;
        public bool AbleToRelease => animationTrait.SkillType is not (SkillType.Instant or SkillType.Casting) && IsActive;
        protected bool IsActive => Sequence == null || Invoker.IsActive;


        public virtual void Initialize()
        {
            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);
            
            detector.Initialize(Cb);
            animationTrait.Initialize(this);
            coolTimer.Initialize(this);
            castTimer.Initialize(this);
            cost.Initialize(this);
            hitExecutor.Initialize(Sequence, this);
            fireExecutor.Initialize(Sequence, this);
            effector.Initialize(Sequence, this);
            
            Builder
                .Add(Section.Active, "StopPathfinding", Cb.Pathfinding.Stop)
                .Add(Section.End,"CharacterStop", Cb.Stop)
                .Add(Section.Release, "ReleaseAction", () =>
                {
                    if (AbleToRelease) 
                        CastTimer.CallbackSection.GetInvokeAction(this)?.Invoke();
                });
        }

        public void Cancel() => Invoker.Cancel();

        public virtual void Dispose()
        {
            // TODO Pool.Release()가 중복으로 들어옴
            // Invoker.End();
            
            Sequence.Clear();
            coolTimer.Dispose();
            castTimer.Dispose();
            
            effector.Dispose();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            hitExecutor.GetExecutionInEditor(transform);
            fireExecutor.GetExecutionInEditor(transform);
            effector.GetEffectsInEditor(transform);
            
            detector.SetUpAsSkill(actionCode);
            coolTimer.SetUpAsSkill(actionCode);
            castTimer.SetUpFromSkill(actionCode);
            cost.SetUpFromSkill(actionCode);
            animationTrait.SetUpFromSkill(actionCode);
            
            var skillData = Database.SkillSheetData(actionCode);
            
            behaviourMask   = skillData.BehaviourMask.ToEnum<ActionMask>();
            priority        = skillData.Priority;
            description     = skillData.Description;
            icon            = Database.SpellSpriteData.Get(actionCode);
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