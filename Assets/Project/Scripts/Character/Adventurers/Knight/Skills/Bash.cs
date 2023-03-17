using Common.Completion;
using Common.Skills;
using UnityEngine;
using UnityEngine.Serialization;

// SkillMechanicEntity;

namespace Character.Adventurers.Knight.Skills
{
    public class Bash : SkillComponent
    {
        [FormerlySerializedAs("power")] [SerializeField] private DamageCompletion damage;
        [SerializeField] private DeBuffCompletion armorCrash;


        protected void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker =>
            {
                damage.Damage(taker);
                armorCrash.DeBuff(taker);
            });
        }

        protected override void Initialize()
        {
            damage.Initialize(Cb, ActionCode);
            armorCrash.Initialize(Cb);
            
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnHit.Register("Bash", OnAttack);
            OnCompleted.Register("EndCallback", End);
            OnEnded.Register("ReleaseHit", UnregisterHitEvent);
        }

        protected override void Dispose()
        {
            // TODO. Unregister Sequence Events;
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = Database.SkillSheetData(actionCode);

            if (!TryGetComponent(out damage)) damage = gameObject.AddComponent<DamageCompletion>();

            damage.SetDamage(skillData.CompletionValueList[0]);

            Database.StatusEffectMaster.Get((DataIndex)skillData.StatusEffect, out var armorCrashObject);
            
            if (!TryGetComponent(out armorCrash)) armorCrash = gameObject.AddComponent<DeBuffCompletion>();

            armorCrash.SetProperties(armorCrashObject, 16);
        }
#endif
    }
}
