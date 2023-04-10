using Singleton;
using UnityEngine;

namespace Common.PlayerCamps
{
    public class PlayerCamp : MonoSingleton<PlayerCamp>, IEditable
    {
        [SerializeField] private CharacterManager characters;
        [SerializeField] private InventoryManager inventories;

        public static CharacterManager Characters => Instance.characters;
        public static InventoryManager Inventories => Instance.inventories;
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            characters  ??= GetComponentInChildren<CharacterManager>();
            inventories ??= GetComponentInChildren<InventoryManager>();
        }
#endif
    }
}
