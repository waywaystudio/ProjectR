using System.Collections.Generic;
using Common.Characters;
using Common.Execution;
using Common.Particles;
using Common.TargetSystem;
using UnityEngine;

namespace Common.Skills
{
    public class SkillComponent : MonoBehaviour, ISkill, IEditable
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected ActionMask behaviourMask = ActionMask.Skill;
        [SerializeField] protected int priority;
        [SerializeField] protected Executor executor;
        [SerializeField] protected CombatTakerDetector detector;
        [SerializeField] protected SkillAnimationTrait animationTrait;
        [SerializeField] protected SkillCoolTimer coolTimer;
        [SerializeField] protected SkillCastTimer castTimer;
        [SerializeField] protected SkillCost cost;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected List<CombatParticle> combatParticles;
        [SerializeField] protected string description;
        
        private CharacterBehaviour cb;
        private readonly SkillSequence sequence = new();

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

        public Sequencer Sequencer => sequence;
        public SkillSequenceBuilder Builder { get; private set; }
        public SkillSequenceInvoker Invoker { get; private set; }
        public SkillCoolTimer CoolTimer => coolTimer;
        public SkillCastTimer CastTimer => castTimer;
        public Vector3 SizeVector => detector.SizeVector;
        public float CastingWeight => CastTimer.CastingTime;
        public float CoolingWeight => CoolTimer.CoolTime;
        
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public bool IsEnded => Sequencer == null || Invoker.IsEnd;
        public bool AbleToRelease => animationTrait.SkillType is not (SkillType.Instant or SkillType.Casting) && IsActive;
        protected bool IsActive => Sequencer == null || Invoker.IsActive;


        public virtual void Initialize()
        {
            Invoker = new SkillSequenceInvoker(sequence);
            Builder = new SkillSequenceBuilder(sequence);
            
            detector.Initialize(Cb);
            animationTrait.Initialize(this);
            coolTimer.Initialize(this);
            castTimer.Initialize(this);
            cost.Initialize(this);
            combatParticles?.ForEach(cp => cp.Initialize(sequence, this));
            
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
            
            sequence.Clear();
            coolTimer.Dispose();
            castTimer.Dispose();
            combatParticles?.ForEach(cp => cp.Dispose());
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
            
            behaviourMask   = skillData.BehaviourMask.ToEnum<ActionMask>();
            priority        = skillData.Priority;
            description     = skillData.Description;
            icon            = Database.SpellSpriteData.Get(actionCode);

            GetComponentsInChildren(combatParticles);
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