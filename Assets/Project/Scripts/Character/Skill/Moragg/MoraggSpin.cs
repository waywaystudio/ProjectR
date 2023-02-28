using Character.Projector;
using Core;
using UnityEngine;

namespace Character.Skill.Moragg
{
    public class MoraggSpin : CastingAttack
    {
        [SerializeField] private SphereProjector projector;
        
        protected override void OnEnable()
        {
            base.OnEnable();

            var self = GetComponentInParent<ICombatTaker>();
            
            projector.Initialize(progressTime, range);
            projector.SetTaker(self);
            
            OnActivated.Register("ProjectorActivate", projector.OnActivated.Invoke);
            OnCanceled.Register("ProjectorInterrupt", projector.OnCanceled.Invoke);
            OnCompleted.Register("ProjectorComplete", projector.OnCompleted.Invoke);
            OnEnded.Register("ProjectorEnd", projector.OnEnded.Invoke);
        }
    }
}
