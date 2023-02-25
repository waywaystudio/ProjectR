using System.Collections.Generic;
using Character;
using Core;
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

        private List<AdventurerBehaviour> adventurerList = new();
        //
        

        public void Initialize()
        {
            var adventurers = RaidDirector.AdventurerList;
            
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
                adventurerList[0].OnProvideCombat.Register("DamageTextUI", ShowDamage);
                adventurerList[0].OnTakeCombat.Register("TakenDamageTextUI", ShowDamage);
            }

            if (showAdventurer2 && adventurerList.Count > 1)
            {
                adventurerList[1].OnProvideCombat.Register("DamageTextUI", ShowDamage);
                adventurerList[1].OnTakeCombat.Register("TakenDamageTextUI", ShowDamage);
            }

            if (showAdventurer3 && adventurerList.Count > 2)
            {
                adventurerList[2].OnProvideCombat.Register("DamageTextUI", ShowDamage);
                adventurerList[2].OnTakeCombat.Register("TakenDamageTextUI", ShowDamage);
            }

            if (showAdventurer4 && adventurerList.Count > 3)
            {
                adventurerList[3].OnProvideCombat.Register("DamageTextUI", ShowDamage);
                adventurerList[3].OnTakeCombat.Register("TakenDamageTextUI", ShowDamage);
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
    }
}
