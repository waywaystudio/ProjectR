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
        
        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

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
        

        public bool TryArm(Equipment equipment)
        {
            // Check AvailableClass
            if(!equipment.AvailableClass.HasFlag(Cb.CombatClass))
            {
                Debug.LogWarning($"Not Available. "
                                 + $"Available Class:{equipment.AvailableClass}. Target Class:{Cb.CombatClass}");
                return false;
            }

            // Check Matched Slot
            var targetSlot = equipment.EquipType switch
            {
                EquipType.Weapon => EquipSlotIndex.Weapon,
                EquipType.Head   => EquipSlotIndex.Head,
                EquipType.Top    => EquipSlotIndex.Top,
                EquipType.Bottom => EquipSlotIndex.Bottom,
                EquipType.Trinket => equipmentTable[EquipSlotIndex.Trinket2].HasEquipment
                    ? EquipSlotIndex.Trinket1
                    : EquipSlotIndex.Trinket2,
                EquipType.None => throw new ArgumentOutOfRangeException(),
                _              => throw new ArgumentOutOfRangeException()
            };
            
            Disarm(targetSlot);

            equipmentTable[targetSlot].Equipment = equipment;
            Cb.StatTable.Add($"{equipment.EquipType}.{targetSlot}", equipment.Spec);

            return true;
        }

        public bool TryArm(EquipSlotIndex slotIndex, Equipment equipment)
        {
            // Check Matched Slot
            if (equipmentTable[slotIndex].EquipType != equipment.EquipType)
            {
                Debug.LogWarning($"Can't Arm Between different SlotType. "
                                 + $"TrySlot:{equipmentTable[slotIndex].EquipType}, TryEquipment:{equipment.EquipType}");
                return false;
            }
            
            // Check AvailableClass
            if(!equipment.AvailableClass.HasFlag(Cb.CombatClass))
            {
                Debug.LogWarning($"Not Available. "
                                 + $"Available Class:{equipment.AvailableClass}. Target Class:{Cb.CombatClass}");
                return false;
            }

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
            
            equipmentTable.ForEach(table
                => table.Value.HasEquipment.OnTrue(() => TryArm(table.Key, table.Value.Equipment)));
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
        }

        private class EquipmentSlot
        {
            [ShowInInspector]
            public Equipment Equipment { get; set; }
            public EquipType EquipType { get; }
            public bool HasEquipment => !Equipment.IsNullOrDestroyed();

            public EquipmentSlot(EquipType equipType)
            {
                EquipType  = equipType;
            }
        }
    }
}
