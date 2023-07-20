using BehaviorDesigner.Runtime.Tasks;
using Common.Characters;
using Common.Characters.Behaviours;
using Common.Skills;

namespace Common.Behavior.Characters
{
    [TaskCategory("Characters")]
    public class Escape : Action
    {
        private CharacterBehaviour Cb { get; set; }

        public override void OnAwake()
        {
            TryGetComponent(out SkillTable sb);

            Cb = sb.Cb;
        }

        public override TaskStatus OnUpdate()
        {
            var characterPosition = Cb.Position;
            var safePosition = PathfindingUtility.GetNearestSafePosition(characterPosition, 1.25f);
            
            Cb.Run(safePosition);
            
            return TaskStatus.Success;
        }
    }
}
