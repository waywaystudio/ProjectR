using Singleton;
using UnityEngine;

namespace Common.PartyCamps
{
    public class PartyCamp : MonoSingleton<PartyCamp>, IEditable
    {
        [SerializeField] private AdventurerManager characters;
        [SerializeField] private InventoryManager inventories;

        public static AdventurerManager Characters => Instance.characters;
        public static InventoryManager Inventories => Instance.inventories;


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            characters  ??= GetComponentInChildren<AdventurerManager>();
            inventories ??= GetComponentInChildren<InventoryManager>();
        }
#endif
    }
}
