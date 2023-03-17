using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Character.Adventurers.Knight.Skills
{
    public class UpperSlash : SkillComponent
    {
        [SerializeField] private DamageCompletion damage;
        [SerializeField] private DeBuffCompletion bleed;


        protected void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker =>
            {
                damage.Damage(taker);
                bleed.DeBuff(taker);
            });
        }
        
        protected override void Initialize()
        {
            damage.Initialize(Cb, ActionCode);
            bleed.Initialize(Cb);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnHit.Register("UpperSlash", OnAttack);
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
            
            if (!TryGetComponent(out bleed)) bleed = gameObject.AddComponent<DeBuffCompletion>();

            bleed.SetProperties(armorCrashObject, 16);
        }
#endif
    }
}
