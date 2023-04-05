using System;
using System.Collections.Generic;
using Common.Equipments;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters
{
    /* Character Equipment는 종국에 빌드에서 Save, Load를 통해 데이터가 통신하므로, 
     * 내부에 SerializeField 된 값들은 큰 의미가 없다. */
    public class CharacterEquipment : MonoBehaviour
    {
        #region ForEditor
        [SerializeField] private DataIndex weaponCode;
        [SerializeField] private DataIndex headCode;
        [SerializeField] private DataIndex topCode;
        [SerializeField] private DataIndex bottomCode;
        [SerializeField] private DataIndex trinket1Code;
        [SerializeField] private DataIndex trinket2Code;
        #endregion

        [ShowInInspector]
        private Dictionary<EquipSlotIndex, EquipmentSlot> equipmentTable { get; } = new(6)
        {
            { EquipSlotIndex.Weapon, new EquipmentSlot(EquipType.Weapon) }, 
            { EquipSlotIndex.Head, new EquipmentSlot(EquipType.Head) }, 
            { EquipSlotIndex.Top, new EquipmentSlot(EquipType.Top) }, 
            { EquipSlotIndex.Bottom, new EquipmentSlot(EquipType.Bottom) }, 
            { EquipSlotIndex.Trinket1, new EquipmentSlot(EquipType.Trinket) }, 
            { EquipSlotIndex.Trinket2, new EquipmentSlot(EquipType.Trinket) },
        };

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public bool TryArm(EquipSlotIndex slotIndex, Equipment equipment)
        {
            // Slot Condition
            if (equipmentTable[slotIndex].SlotType != equipment.EquipType)
            {
                Debug.LogWarning($"Can't Arm Between different SlotType. "
                                 + $"TrySlot:{equipmentTable[slotIndex].SlotType}, TryEquipment:{equipment.EquipType}");
                return false;
            }
            
            // Add AvailableClassCondition
            

            Disarm(slotIndex);

            equipmentTable[slotIndex].Equipment = equipment;
            Cb.StatTable.Add($"{equipment.EquipType}.{slotIndex}", equipment.Spec);

            return true;
        }

        public void Disarm(EquipSlotIndex slotIndex)
        {
            if (!equipmentTable[slotIndex].HasEquipment) return;

            var currentEquipment = equipmentTable[slotIndex].Equipment;
            
            Cb.StatTable.Remove($"{currentEquipment.EquipType}.{slotIndex}", currentEquipment.Spec);
            equipmentTable[slotIndex].Equipment = null;
        }

        public void Load()
        {
            // TODO. After Save&Load Complete
            LoadEquipment(weaponCode,   EquipSlotIndex.Weapon);
            LoadEquipment(headCode,     EquipSlotIndex.Head);
            LoadEquipment(topCode,      EquipSlotIndex.Top);
            LoadEquipment(bottomCode,   EquipSlotIndex.Bottom);
            LoadEquipment(trinket1Code, EquipSlotIndex.Trinket1);
            LoadEquipment(trinket2Code, EquipSlotIndex.Trinket2);
        }


        private void LoadEquipment(DataIndex dataCode, EquipSlotIndex slotIndex)
        {
            if (dataCode == DataIndex.None) return;
            
            Database.EquipmentMaster.GetObject(dataCode, out var equipmentPrefab)
                    .OnFalse(() => Debug.LogWarning($"Not Exist {dataCode} prefab")); 
                
            equipmentTable[slotIndex].Equipment = Instantiate(equipmentPrefab, transform).GetComponent<Equipment>();
        }

        private void Awake()
        {
            Load();
            
            equipmentTable.ForEach(table =>
            {
                if (table.Value.HasEquipment)
                {
                    TryArm(table.Key, table.Value.Equipment);
                }
            });
        }

        [Serializable]
        private class EquipmentSlot
        {
            [SerializeField] private EquipType slotType;
            [SerializeField] private Equipment equipment;

            public EquipType SlotType => slotType;
            public Equipment Equipment { get => equipment; set => equipment = value; }
            public bool HasEquipment => !Equipment.IsNullOrDestroyed();

            public EquipmentSlot(EquipType slotType)
            {
                this.slotType  = slotType;
            }
        }
    }
}
