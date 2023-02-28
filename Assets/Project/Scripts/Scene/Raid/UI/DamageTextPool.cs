using System.Collections.Generic;
using Character;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid.UI
{
    public class DamageTextPool : Pool<DamageTextUI>
    {
        // TODO.TEMP
        [SerializeField] private bool showAdventurer1;
        [SerializeField] private bool showAdventurer2;
        [SerializeField] private bool showAdventurer3;
        [SerializeField] private bool showAdventurer4;

        private List<Adventurer> adventurerList = new();
        //
        

        public void Initialize(List<Adventurer> adventurers)
        {
            if (adventurers.IsNullOrEmpty())
            {
                Debug.LogWarning("AtLeast 1 Adventurer Required. DamageTextUI Off.");
                return;
            }
            
            if (adventurers.Count < 4)
            {
                Debug.LogWarning($"Recommend 4 more Adventurer. Input:adventurer Count {adventurers.Count}");
            }

            adventurerList = adventurers;

            if (showAdventurer1 && adventurerList.Count > 0)
            {
                adventurerList[0].OnProvideDamage.Register("DamageTextUI", ShowDamage);
                adventurerList[0].OnTakeDamage.Register("TakenDamageTextUI", ShowDamage);
            }

            if (showAdventurer2 && adventurerList.Count > 1)
            {
                adventurerList[1].OnProvideDamage.Register("DamageTextUI", ShowDamage);
                adventurerList[1].OnTakeDamage.Register("TakenDamageTextUI", ShowDamage);
            }

            if (showAdventurer3 && adventurerList.Count > 2)
            {
                adventurerList[2].OnProvideDamage.Register("DamageTextUI", ShowDamage);
                adventurerList[2].OnTakeDamage.Register("TakenDamageTextUI", ShowDamage);
            }

            if (showAdventurer4 && adventurerList.Count > 3)
            {
                adventurerList[3].OnProvideDamage.Register("DamageTextUI", ShowDamage);
                adventurerList[3].OnTakeDamage.Register("TakenDamageTextUI", ShowDamage);
            }
        }
        

        protected override void OnGetPool(DamageTextUI damageText)
        {
            damageText.transform.SetParent(Origin);
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

            damageText.ShowValue(combatEntity);
            damageText.gameObject.SetActive(true);
        }

        #region TEMP

        [Button] private void TogglePlayer1()
        {
            showAdventurer1 = !showAdventurer1;

            if (showAdventurer1) 
                adventurerList[0].OnProvideDamage.Register("DamageTextUI", ShowDamage);
            else
                adventurerList[0].OnProvideDamage.Unregister("DamageTextUI");
        }
        
        [Button] private void TogglePlayer2()
        {
            showAdventurer2 = !showAdventurer2;

            if (showAdventurer2) 
                adventurerList[1].OnProvideDamage.Register("DamageTextUI", ShowDamage);
            else
                adventurerList[1].OnProvideDamage.Unregister("DamageTextUI");
        }
        
        [Button] private void TogglePlayer3()
        {
            showAdventurer3 = !showAdventurer3;

            if (showAdventurer3) 
                adventurerList[2].OnProvideDamage.Register("DamageTextUI", ShowDamage);
            else
                adventurerList[2].OnProvideDamage.Unregister("DamageTextUI");
        }
        
        [Button] private void TogglePlayer4()
        {
            showAdventurer4 = !showAdventurer4;

            if (showAdventurer4) 
                adventurerList[3].OnProvideDamage.Register("DamageTextUI", ShowDamage);
            else
                adventurerList[3].OnProvideDamage.Unregister("DamageTextUI");
        }

        #endregion
    }
}
