// using Core;
//
// namespace Character.Combat.Projector
// {
//     public class MeteorStrikeProjector : ProjectorObject
//     {
//         protected override void EnterProjector(ICombatTaker taker)
//         {
//             /* --------------------- */
//             if (!taker.Object.TryGetComponent(out CharacterBehaviour cb)) return;
//             
//             cb.CancelSkill();
//         }
//
//         protected override void EndProjector(ICombatTaker taker)
//         {
//             if (DamageModule) taker.TakeDamage(DamageModule);
//         }
//     }
// }
//
