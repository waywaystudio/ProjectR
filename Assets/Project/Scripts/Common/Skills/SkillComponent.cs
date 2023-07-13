using Common.Characters;
using Common.Effects;
using Common.Execution;
using Common.TargetSystem;
using Inputs;
using UnityEngine;

namespace Common.Skills
{
    public class SkillComponent : MonoBehaviour, ISkill, IEditable
    {
        [SerializeField] protected DataIndex dataIndex;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected Effector effector; 
        [SerializeField] protected CombatTakerDetector detector;
        [SerializeField] protected SkillAnimationTrait animationTrait;
        [SerializeField] protected SkillCoolTimer coolTimer;
        [SerializeField] protected SkillCastTimer castTimer;
        [SerializeField] protected SkillCost cost;
        [SerializeField] protected int priority;
        [SerializeField] protected string description;
        
        private CharacterBehaviour cb;

        public DataIndex DataIndex => dataIndex;
        public ActionMask BehaviourMask => behaviourMask;
        public ICombatProvider Provider => Cb;
        public ICombatTaker Taker { get; protected set; }
        public ICombatTaker MainTarget => detector?.GetMainTarget();
        public Sprite Icon => icon;
        public int Priority => priority;
        public string Description => description;
        public float Haste => Cb.StatTable.Haste;
        public float Distance => detector.Distance;
        public float Range => detector.Range;
        public float Angle => detector.Angle;

        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        
        [Sirenix.OdinInspector.ShowInInspector]
        public CombatSequence Sequence { get; } = new();
        public CombatSequenceBuilder Builder { get; private set; }
        public CombatSequenceInvoker Invoker { get; private set; }
        public SkillCoolTimer CoolTimer => coolTimer;
        public SkillCastTimer CastTimer => castTimer;
        public Vector3 SizeVector => detector.SizeVector;
        public float CastingWeight => CastTimer.CastingTime;
        public float CoolingWeight => CoolTimer.CoolTime;
        public bool AbleToRelease => animationTrait.SkillType is not (SkillType.Instant or SkillType.Casting) 
                                     && Invoker.IsActive;

        public virtual void Initialize()
        {
            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);
            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(cb.BehaviourMask))
                .Add(Section.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                .Add(Section.Active, "StopPathfinding", Cb.Pathfinding.Stop)
                .Add(Section.Release, "ReleaseAction", () => AbleToRelease.OnTrue(() => CastTimer.CallbackSection.GetCombatInvoker(Invoker)?.Invoke()))
                .Add(Section.End,"CharacterStop", Cb.Stop)
                ;
            
            detector.Initialize(Cb);
            animationTrait.Initialize(this);
            coolTimer.Initialize(this);
            castTimer.Initialize(this);
            cost.Initialize(this);
            hitExecutor.Initialize(Sequence, this);
            fireExecutor.Initialize(Sequence, this);
            effector.Initialize(Sequence, this);
            
            // InputManager.Raid[InputSection.Start, "Q"].Add("", () => Debug.Log("Q!"));
        }

        public void Cancel() => Invoker.Cancel();

        public void ActiveEffect(bool activity)
        {
            effector.ActiveEffect(activity);
        }
        

        protected virtual void Dispose()
        {
            // Invoker.End();
            
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
            
            behaviourMask   = skillData.BehaviourMask.ToEnum<ActionMask>();
            priority        = skillData.Priority;
            description     = skillData.Description;
            icon            = Database.SpellSpriteData.Get(dataIndex);
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