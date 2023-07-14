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


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            VenturerBehaviour newFocusedAdventurer = null;
            
            foreach (var adventurer in RaidDirector.VenturerList)
            {
                if (adventurer != vb) continue;
                
                newFocusedAdventurer = adventurer;
                break;
            }

            if (newFocusedAdventurer is null) return;
            if (focusedAdventurer is not null)
            {
                focusedAdventurer.OnDamageProvided.Remove("DamageTextUI");
                focusedAdventurer.OnDamageTaken.Remove("TakenDamageTextUI");
            }
            
            newFocusedAdventurer.OnDamageProvided.Add("DamageTextUI", ShowDamage);
            newFocusedAdventurer.OnDamageTaken.Add("TakenDamageTextUI", ShowDamage);
        }


        private void ShowDamage(CombatEntity combatEntity)
        {
            var damageText = pool.Get();

            damageText.ShowValue(combatEntity);
        }

        private void CreateFloatingTextUI(FloatingTextUI textUIComponent)
        {
            textUIComponent.OnEnded.Add("ReleasePool", () => pool.Release(textUIComponent));
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
            
            RaidDirector.VenturerList.ForEach(adventurer =>
            {
                adventurer.OnDamageProvided.Remove("DamageTextUI");
                adventurer.OnDamageTaken.Remove("TakenDamageTextUI");
            });
        }


        #region TEMP
        [Button] private void TogglePlayer1()
        {
            if (RaidDirector.VenturerList.Count < 0) return;
            
            RaidDirector.VenturerList[0].OnDamageProvided.Add("DamageTextUI", ShowDamage);
            RaidDirector.VenturerList[0].OnDamageTaken.Add("TakenDamageTextUI", ShowDamage);
        }
        
        [Button] private void TogglePlayer2()
        {
            if (RaidDirector.VenturerList.Count < 1) return;
            
            RaidDirector.VenturerList[1].OnDamageProvided.Add("DamageTextUI", ShowDamage);
            RaidDirector.VenturerList[1].OnDamageTaken.Add("TakenDamageTextUI", ShowDamage);
        }
        
        [Button] private void TogglePlayer3()
        {
            if (RaidDirector.VenturerList.Count < 2) return;
            
            RaidDirector.VenturerList[2].OnDamageProvided.Add("DamageTextUI", ShowDamage);
            RaidDirector.VenturerList[2].OnDamageTaken.Add("TakenDamageTextUI", ShowDamage);
        }
        
        [Button] private void TogglePlayer4()
        {
            if (RaidDirector.VenturerList.Count < 3) return;
            
            RaidDirector.VenturerList[3].OnDamageProvided.Add("DamageTextUI", ShowDamage);
            RaidDirector.VenturerList[3].OnDamageTaken.Add("TakenDamageTextUI", ShowDamage);
        }
        #endregion
    }
}
