using Common;
using UnityEngine;

namespace Lobby
{
    public class LobbyCastingDirector : MonoBehaviour
    {
        [SerializeField] private Transform venturerHierarchy;
        
        public void Initialize()
        {
            SpawnVenturer(VenturerType.Knight);
        }
        
        
        private void SpawnVenturer(VenturerType venturerEntry)
        {
            if (venturerEntry == VenturerType.None) return;

            var profitPosition = Vector3.zero;

            Camp.Spawn(venturerEntry, profitPosition, venturerHierarchy, out var vb);
            LobbyDirector.FocusVenturer = vb;
        }
    }
}
