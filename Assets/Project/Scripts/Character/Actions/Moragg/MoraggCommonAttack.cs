using Character.Actions;
using UnityEngine;

namespace Character.Skill.Moragg
{
    public class MoraggCommonAttack : GeneralAttack
    {
        [SerializeField] protected ValueCompletion power;
        
        protected override void OnAttack()
        {
            power.Damage(MainTarget);
        }
        
        protected override void OnEnable()
        {
            power.Initialize(Provider, ActionCode);

            base.OnEnable();
        }
    }
}
