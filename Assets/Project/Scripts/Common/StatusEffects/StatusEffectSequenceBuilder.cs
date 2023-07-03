using System;
using UnityEngine;

namespace Common.StatusEffects
{
    // public class StatusEffectSequencer : Sequencer
    // {
    //     public ActionTable<ICombatTaker> TakerExecute1Section { get; } = new();
    //     public ActionTable<ICombatTaker> TakerExecute2Section { get; } = new();
    // }
    //
    // public class StatusEffectSequenceBuilder : SequenceBuilder
    // {
    //     private readonly StatusEffectSequencer statusEffectSequencer;
    //     
    //     public StatusEffectSequenceBuilder(StatusEffectSequencer holder) : base(holder)
    //     {
    //         statusEffectSequencer = holder;
    //     }
    //
    //     public SequenceBuilder AddExecution(ExecuteGroup group, string key, Action<ICombatTaker> action)
    //     {
    //         switch (group)
    //         {
    //             case ExecuteGroup.Group1:
    //             {
    //                 statusEffectSequencer.TakerExecute1Section.Add(key, action);
    //                 break;
    //             } 
    //             case ExecuteGroup.Group2:
    //             {
    //                 statusEffectSequencer.TakerExecute2Section.Add(key, action);
    //                 break;
    //             }
    //             default:
    //             {
    //                 Debug.LogWarning("StatusEffectSequenceBuilder support only Group1 and 2");
    //                 break;
    //             }
    //         }
    //
    //         return this;
    //     }
    // }
    
    
}
