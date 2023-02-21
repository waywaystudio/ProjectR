using Character;
using Core;

namespace Raid.UI
{
    public class DamageTextPool : Pool<DamageText>
    {
        // TODO.TEMP
        public AdventurerBehaviour FirstAdventurer;
        public AdventurerBehaviour SecondAdventurer;
        public AdventurerBehaviour ThirdAdventurer;
        public AdventurerBehaviour FourthAdventurer;

        public bool ShowFirstAdventurerDamage;
        public bool ShowSecondAdventurerDamage;
        public bool ShowThirdAdventurerDamage;
        public bool ShowFourthAdventurerDamage;
        //

        public void OnStatusEffectGet(DamageText damageText)
        {
            damageText.gameObject.SetActive(true);
            damageText.transform.SetParent(parent);
        }
        
        public void OnStatusEffectRelease(DamageText damageText)
        {
            damageText.gameObject.SetActive(false);
            damageText.transform.SetParent(Origin);
        }
        
        public void OnStatusEffectDestroy(DamageText damageText)
        {
            Destroy(damageText.gameObject);
        }

        private void ShowDamage(CombatEntity combatEntity)
        {
            var damageText = Get();
            
            damageText.ShowValue(combatEntity);
        }

        private void OnEnable()
        {
            FirstAdventurer.OnProvideCombat.Register("ShowDamageText", ShowDamage);
        }
    }
}
