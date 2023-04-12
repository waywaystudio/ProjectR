using System.Collections.Generic;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterData : ScriptableObject, ISavable
    {
        [SerializeField] private CombatClassType classType;
        [SerializeField] private string characterName;
        
        private EquipmentInfo weaponInfo;
        private EquipmentInfo headInfo;
        private EquipmentInfo topInfo;
        private EquipmentInfo bottomInfo;
        private EquipmentInfo trinket1Info;
        private EquipmentInfo trinket2Info;

        public CombatClassType ClassType => classType;
        private string SerializeKey => $"{characterName}'s Equipments";

        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<EquipSlotIndex, Equipment> Table { get; private set; } = new();


        public void Save()
        {
            SaveManager.Save(SerializeKey, Table);
        }

        public void Load()
        {
            var tableData = SaveManager.Load<Dictionary<EquipSlotIndex, Equipment>>(SerializeKey);

            if (tableData.IsNullOrEmpty()) return;
            
            Table = tableData;
        }
    }
}
