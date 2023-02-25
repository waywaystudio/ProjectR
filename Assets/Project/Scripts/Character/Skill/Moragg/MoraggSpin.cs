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
            projector.AssignTo(this);
            projector.SetTaker(self);
        }
    }
}
