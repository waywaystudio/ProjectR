using System;
using Common.Characters;
using Common.Effects;
using Common.Execution;
using Common.Detects;
using UnityEngine;

namespace Common.Skills
{
    public class SkillComponent : MonoBehaviour, ICombatObject, IActionBehaviour, IProjection, IEditable
    {
        [SerializeField] protected DataIndex dataIndex;
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected Effector effector; 
        [SerializeField] protected TakerDetector detector;
        [SerializeField] protected SkillAnimationTrait animationTrait;
        [SerializeField] protected SkillCoolTimer coolTimer;
        [SerializeField] protected SkillCastTimer castTimer;
        [SerializeField] protected SkillCost cost;
        [SerializeField] protected int priority;
        [SerializeField] protected string description;
        
        private CharacterBehaviour cb;

        public DataIndex DataIndex => dataIndex;
        public ActionMask BehaviourMask => behaviourMask;
        public SkillType SkillType => skillType;
        public ICombatProvider Provider => Cb;
        public ICombatTaker Taker { get; protected set; }
        public ICombatTaker MainTarget => detector?.GetMainTarget();
        public Sprite Icon => icon;
        public int Priority => priority;
        public string Description => description;
        public Func<float> Haste => () => Cb.StatTable.Haste;
        public float PivotRange => SizeEntity.PivotRange;
        public float AreaRange => SizeEntity.AreaRange;
        public float Angle => SizeEntity.Angle;
        public float Height => SizeEntity.Height;
        public float Width => SizeEntity.Width;
        public float CastingTime => CastTimer.Duration;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        public CombatSequence Sequence { get; } = new();
        public CombatSequenceInvoker Invoker { get; private set; }
        public CombatSequenceBuilder Builder { get; private set; }
        public TakerDetector Detector => detector;
        public SkillCoolTimer CoolTimer => coolTimer;
        public SkillCastTimer CastTimer => castTimer;
        public SkillCost SkillCost => cost;
        public SizeEntity SizeEntity => detector.SizeEntity;


        public virtual void Initialize()
        {
            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);
            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(cb.BehaviourMask))
                .Add(Section.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                .Add(Section.Active, "StopPathfinding", Cb.Pathfinding.Stop)
                .Add(Section.End,"CharacterStop", Cb.Stop)
                ;
            
            detector.Initialize(Cb);
            animationTrait.Initialize(this);
            coolTimer.Initialize(this);
            castTimer.Initialize(this);
            cost.Initialize(this);
            hitExecutor.Initialize(this);
            fireExecutor.Initialize(this);
            effector.Initialize(this);
        }

        public void Cancel() => Invoker.Cancel();

        public void ActiveEffect(bool activity)
        {
            effector.ActiveEffect(activity);
        }
        

        protected virtual void Dispose()
        {
            Sequence.Clear();
            coolTimer.Dispose();
            castTimer.Dispose();
        }
        

        private void OnDestroy()
        {
            Dispose();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            hitExecutor.GetExecutionInEditor(transform);
            fireExecutor.GetExecutionInEditor(transform);
            effector.GetEffectsInEditor(transform);
            detector.SetUpAsSkill(dataIndex);
            coolTimer.SetUpAsSkill(dataIndex);
            castTimer.SetUpFromSkill(dataIndex);
            cost.SetUpFromSkill(dataIndex);
            animationTrait.SetUpFromSkill(dataIndex);
            
            var skillData = Database.SkillSheetData(dataIndex);
            
            skillType     = skillData.SkillType.ToEnum<SkillType>();
            behaviourMask = skillData.BehaviourMask.ToEnum<ActionMask>();
            priority      = skillData.Priority;
            description   = skillData.Description;
            icon          = Database.SpellSpriteData.Get(dataIndex);
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