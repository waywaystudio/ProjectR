using UnityEngine;
using UnityEngine.Events;

namespace Character.Adventurers
{
    public class AdventurerModChanger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onManual;
        [SerializeField] private UnityEvent onAuto;

        private Adventurer adventurer;
        private Adventurer Adventurer => adventurer ??= GetComponent<Adventurer>();
        

        public void OnFocused(Adventurer focusAdventurer)
        {
            if (Adventurer is null) return;

            if (focusAdventurer == Adventurer)
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
