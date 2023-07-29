using Character.Venturers;
using Common;
using UnityEngine;

namespace Lobby
{
    public class LobbyCastingDirector : MonoBehaviour
    {
        [SerializeField] private Transform venturerHierarchy;

        public VenturerBehaviour PlayerVenturer { get; set; }


        public void Initialize()
        {
            SpawnVenturer(VenturerType.Warrior);
        }


        private void SpawnVenturer(VenturerType venturerEntry)
        {
            var profitPosition = Vector3.zero;

            Camp.Spawn(venturerEntry, profitPosition, venturerHierarchy, out var vb);
            PlayerVenturer = vb;
        }
    }
}
