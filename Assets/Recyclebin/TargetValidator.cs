// using BehaviorDesigner.Runtime.Tasks;
// using Character.Combat;
// using Core;
//
// namespace Character.Behavior.Actions.Combat
// {
//     [TaskCategory("Character/Combat")]
//     public class TargetValidator : Action
//     {
//         private ICombatTaker taker;
//         private CombatBehaviour combat;
//         
//         public override void OnAwake()
//         {
//             combat = GetComponent<CombatBehaviour>();
//         }
//         
//         public override TaskStatus OnUpdate()
//         {
//             if (!combat.TryGetMostPrioritySkill(out var skill))
//             {
//                 return TaskStatus.Failure;
//             }
//
//             var targetModule = skill.TargetModule;
//
//             if (targetModule is null)
//             {
//                 return TaskStatus.Success;
//             }
//
//             taker = targetModule.Target;
//
//             var isValid = taker != null && taker.DynamicStatEntry.IsAlive.Value;
//
//             return isValid
//             ? TaskStatus.Success
//             : TaskStatus.Failure;
//         }
//     }
// }
