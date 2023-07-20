using BehaviorDesigner.Runtime.Tasks;
using Common.Characters.Behaviours;
using Common.Skills;

namespace Common.Behavior.Characters
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Characters")]
    public class IsSkillFinished : Action
    {
        private SkillTable sb;
        
        public override void OnAwake()
        {
            TryGetComponent(out sb);
        }
        
        public override TaskStatus OnUpdate() => sb.Current is null ||  sb.Current.Invoker.IsEnd
            ? TaskStatus.Success 
            : TaskStatus.Failure;
    }
}
