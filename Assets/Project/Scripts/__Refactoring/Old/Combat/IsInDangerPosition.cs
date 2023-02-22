// using BehaviorDesigner.Runtime.Tasks;
//
// namespace Character.Behavior.Combat
// {
//     [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
//     public class IsInDangerPosition : Action
//     {
//         private CharacterBehaviour cb;
//         
//         public override void OnAwake()
//         {
//             cb = gameObject.GetComponentInParent<CharacterBehaviour>();
//         }
//         
//         public override TaskStatus OnUpdate()
//         {
//             var isInDanger = !cb.PathfindingEngine.IsSafe;
//
//             return isInDanger
//                 ? TaskStatus.Success
//                 : TaskStatus.Failure;
//         }
//     }
// }
