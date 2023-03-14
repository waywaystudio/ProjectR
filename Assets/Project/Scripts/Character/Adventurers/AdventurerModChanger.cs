using Character.Adventurers;
using UnityEngine;
using UnityEngine.Events;

namespace Adventurers
{
    public class AdventurerModChanger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onManual;
        [SerializeField] private UnityEvent onAuto;

        private Adventurer adventurer;
        

        public void Initialize(Adventurer adventurer)
        {
            this.adventurer = adventurer;
        }

        public void OnFocused(Adventurer focusAdventurer)
        {
            if (adventurer is null) return;

            if (focusAdventurer == adventurer)
            {
                onManual?.Invoke();
            }
            else
            {
                onAuto?.Invoke();
            }
        }
    }
}
