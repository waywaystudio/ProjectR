// using BehaviorDesigner.Runtime.Tasks;
// using Core;
//
// namespace Character.Behavior.Combat
// {
//     [TaskCategory("Character/Combat")]
//     public class WaitUntilReached : Action
//     {
//         private CharacterBehaviour cb;
//         private IPathfinding pathfindingEngine;
//         
//         public override void OnAwake()
//         {
//             cb                = gameObject.GetComponentInParent<CharacterBehaviour>();
//             pathfindingEngine = cb.PathfindingEngine;
//         }
//
//         public override TaskStatus OnUpdate()
//         {
//             return pathfindingEngine.IsReached
//                 ? TaskStatus.Success
//                 : TaskStatus.Failure;
//         }
//
//     }
// }
