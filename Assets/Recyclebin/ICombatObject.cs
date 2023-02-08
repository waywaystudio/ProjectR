// using System.Collections.Generic;
// using Core;
//
// namespace Character.Combat
// {
//     public interface ICombatObject : IActionSender
//     {
//         // + ICombatProvider Provider { get; }
//         // + IDCode ActionCode { get; }
//         
//         public ActionTable OnActivated { get; }
//         public ActionTable OnCompleted { get; }
//         public ActionTable OnCanceled { get; }
//         public ActionTable OnHit { get; }
//     
//         List<IReady> ReadyCheckList { get; }
//         Dictionary<ModuleType, CombatModule> ModuleTable { get; }
//     
//         void Active();
//         void Complete();
//         void Cancel();
//         void Hit();
//     }
// }
