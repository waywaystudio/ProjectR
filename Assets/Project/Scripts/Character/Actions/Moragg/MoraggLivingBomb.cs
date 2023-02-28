using Character.StatusEffect;
using UnityEngine;

namespace Character.Actions.Moragg
{
    public class MoraggLivingBomb : GeneralAttack
    {
        [SerializeField] private StatusEffectCompletion livingBomb;


        protected override void OnAttack()
        {
            if (MainTarget is null) return;
            
            livingBomb.DeBuff(MainTarget);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            OnCompleted.Register("StartCooling", StartCooling);
        }
    }
}
