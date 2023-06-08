using Character.Venturers;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid.UI.FloatingTexts
{
    public class FloatingTextPool : MonoBehaviour 
    {
        [SerializeField] private Pool<FloatingTextUI> pool;
        
        private VenturerBehaviour focusedAdventurer;


        public void OnFocusingAdventurer(VenturerBehaviour focusAdventurer)
        {
            VenturerBehaviour newFocusedAdventurer = null;
            
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


        private void ShowDamage(CombatEntity combatEntity)
        {
            var damageText = pool.Get();

            damageText.ShowValue(combatEntity);
        }

        private void CreateFloatingTextUI(FloatingTextUI textUIComponent)
        {
            textUIComponent.OnEnded.Register("ReleasePool", () => pool.Release(textUIComponent));
        }

        private void Awake()
        {
            pool.Initialize(CreateFloatingTextUI, transform);
        }

        private void OnDestroy()
        {
            if (RaidDirector.Instance.IsNullOrDestroyed())
            {
                Debug.Log("RaidDirector Destroyed");
                return;
            }
            
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
