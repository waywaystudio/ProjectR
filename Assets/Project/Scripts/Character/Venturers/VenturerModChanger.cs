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
        

        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (Adventurer is null) return;

            if (vb == Adventurer)
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
