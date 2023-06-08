using UnityEngine;
using UnityEngine.Events;

namespace Character.Venturers
{
    public class VenturerModChanger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onManual;
        [SerializeField] private UnityEvent onAuto;

        private VenturerBehaviour adventurer;
        private VenturerBehaviour Adventurer => adventurer ??= GetComponent<VenturerBehaviour>();
        

        public void OnFocused(VenturerBehaviour focusAdventurer)
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
