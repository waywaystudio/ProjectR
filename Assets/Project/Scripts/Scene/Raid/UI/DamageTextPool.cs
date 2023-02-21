using Character;
using Core;

namespace Raid.UI
{
    public class DamageTextPool : Pool<DamageTextUI>
    {
        // TODO.TEMP
        public AdventurerBehaviour FirstAdventurer;
        //

        protected override void OnGetPool(DamageTextUI damageText)
        {
            damageText.gameObject.SetActive(true);
        }
        
        protected override void OnReleasePool(DamageTextUI damageText)
        {
            damageText.gameObject.SetActive(false);
            damageText.transform.SetParent(Origin);
        }
        
        protected override void OnDestroyPool(DamageTextUI damageText)
        {
            Destroy(damageText.gameObject);
        }

        private void ShowDamage(CombatEntity combatEntity)
        {
            var damageText = Get();
            
            damageText.transform.SetParent(combatEntity.Taker.DamageSpawn, false);
            damageText.ShowValue(combatEntity);
        }

        private void OnEnable()
        {
            FirstAdventurer.OnProvideCombat.Register("ShowDamageText", ShowDamage);
        }
    }
}
