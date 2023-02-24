using Character.StatusEffect;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Skill.Moragg
{
    public class MoraggLivingBomb : GeneralAttack
    {
        [SerializeField] private StatusEffectCompletion livingBomb;


        protected override void OnAttack()
        {
            if (MainTarget is null) return;
            
            livingBomb.Effect(MainTarget);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            OnCompleted.Register("StartCooling", StartCooling);
        }
    }
}
