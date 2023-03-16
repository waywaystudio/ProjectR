using Adventurers;
using Character.Adventurers;
using Common;
using Sirenix.OdinInspector;

namespace Raid.UI.FloatingTexts
{
    public class FloatingTextPool : Pool<FloatingTextUI>
    {
        private Adventurer focusedAdventurer;


        public void OnFocusingAdventurer(Adventurer focusAdventurer)
        {
            Adventurer newFocusedAdventurer = null;
            
            foreach (var adventurer in RaidDirector.AdventurerList)
            {
                if (adventurer != focusAdventurer) continue;
                
                newFocusedAdventurer = adventurer;
                break;
            }

            if (newFocusedAdventurer is null) return;
            if (focusedAdventurer is not null)
            {
                focusedAdventurer.OnDamageProvided.Unregister("DamageTextUI");
                focusedAdventurer.OnDamageTaken.Unregister("TakenDamageTextUI");
            }
            
            newFocusedAdventurer.OnDamageProvided.Register("DamageTextUI", ShowDamage);
            newFocusedAdventurer.OnDamageTaken.Register("TakenDamageTextUI", ShowDamage);
        }
        

        protected override void OnGetPool(FloatingTextUI floatingText)
        {
            floatingText.transform.SetParent(transform);
        }
        
        protected override void OnReleasePool(FloatingTextUI floatingText)
        {
            floatingText.gameObject.SetActive(false);
            floatingText.transform.SetParent(transform);
        }
        
        protected override void OnDestroyPool(FloatingTextUI floatingText)
        {
            Destroy(floatingText.gameObject);
        }

        private void ShowDamage(CombatEntity combatEntity)
        {
            var damageText = Get();

            damageText.ShowValue(combatEntity);
            damageText.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            RaidDirector.AdventurerList.ForEach(adventurer =>
            {
                adventurer.OnDamageProvided.Unregister("DamageTextUI");
                adventurer.OnDamageTaken.Unregister("TakenDamageTextUI");
            });
        }


        #region TEMP
        [Button] private void TogglePlayer1()
        {
            if (RaidDirector.AdventurerList.Count < 0) return;
            
            RaidDirector.AdventurerList[0].OnDamageProvided.Register("DamageTextUI", ShowDamage);
            RaidDirector.AdventurerList[0].OnDamageTaken.Register("TakenDamageTextUI", ShowDamage);
        }
        
        [Button] private void TogglePlayer2()
        {
            if (RaidDirector.AdventurerList.Count < 1) return;
            
            RaidDirector.AdventurerList[1].OnDamageProvided.Register("DamageTextUI", ShowDamage);
            RaidDirector.AdventurerList[1].OnDamageTaken.Register("TakenDamageTextUI", ShowDamage);
        }
        
        [Button] private void TogglePlayer3()
        {
            if (RaidDirector.AdventurerList.Count < 2) return;
            
            RaidDirector.AdventurerList[2].OnDamageProvided.Register("DamageTextUI", ShowDamage);
            RaidDirector.AdventurerList[2].OnDamageTaken.Register("TakenDamageTextUI", ShowDamage);
        }
        
        [Button] private void TogglePlayer4()
        {
            if (RaidDirector.AdventurerList.Count < 3) return;
            
            RaidDirector.AdventurerList[3].OnDamageProvided.Register("DamageTextUI", ShowDamage);
            RaidDirector.AdventurerList[3].OnDamageTaken.Register("TakenDamageTextUI", ShowDamage);
        }
        #endregion
    }
}
