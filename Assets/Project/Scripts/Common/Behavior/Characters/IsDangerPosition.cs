using BehaviorDesigner.Runtime.Tasks;
using Common.Characters;
using Common.Skills;
using Common.Systems;

namespace Common.Behavior.Characters
{
    [TaskCategory("Characters")]
    public class IsDangerPosition : Action
    {
        private CharacterBehaviour Cb { get; set; }
        private PathfindingSystem Path { get; set; }

        public override void OnAwake()
        {
            TryGetComponent(out SkillTable st);

            Cb = st.Cb;
            Path = Cb.Pathfinding;
        }
        
        public override TaskStatus OnUpdate()
        {
            if (Cb.CombatClass == CharacterMask.Knight) return TaskStatus.Failure;
            
            return Path.IsSafe 
                ? TaskStatus.Failure 
                : TaskStatus.Success;
        }
    }
}
