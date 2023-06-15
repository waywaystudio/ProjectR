using Common;
using Common.Traps;
using UnityEngine;

namespace Character.Villains.Moragg.Projector
{
    public class Meteor : TrapComponent, IProjectorSequence
    {
        public Vector2 SizeVector => new (radius * 2f, radius * 2f);
        public float CastingTime => ProlongTime;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            OnCompleted.Register("Execution", Execution);
        }
        
        public override void Execution()
        {
            if (TryGetTakerInSphere(out var takerList))
            {
                takerList.ForEach(ExecutionTable.Execute);
            }
        }

        public ConditionTable Condition { get; }
        public ActionTable ActiveAction { get; }
        public ActionTable CancelAction { get; }
        public ActionTable CompleteAction { get; }
        public ActionTable EndAction { get; }
    }
}
