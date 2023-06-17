using Common;
using Common.Traps;
using UnityEngine;

namespace Character.Villains.Moragg.Projector
{
    public class Meteor : TrapComponent, IProjectorSections
    {
        public Vector2 SizeVector => new (radius * 2f, radius * 2f);
        public float CastWeightTime => ProlongTime;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            sequencer.CompleteAction.Add("Execute", Execution);
            
            ExecuteAction.Add("MeteorExecution", () =>
            {
                if (TryGetTakerInSphere(out var takerList))
                {
                    takerList.ForEach(executor.Execute);
                }
            });
        }

        public override void Execution() => ExecuteAction.Invoke();
    }
}
