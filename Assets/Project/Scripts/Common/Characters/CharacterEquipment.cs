using System;
using System.Collections.Generic;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterEquipment : MonoBehaviour, ISavable
    {
        /* Initial Equipment */
        [SerializeField] private EquipmentInfo weaponInfo;
        [SerializeField] private EquipmentInfo headInfo;
        [SerializeField] private EquipmentInfo topInfo;
        [SerializeField] private EquipmentInfo bottomInfo;
        [SerializeField] private EquipmentInfo trinket1Info;
        [SerializeField] private EquipmentInfo trinket2Info;
        

        private CharacterBehaviour cb;
        public Dictionary<EquipSlotIndex, Equipment> Table { get; } = new(6)
        {
            { EquipSlotIndex.Weapon, null }, 
            { EquipSlotIndex.Head, null }, 
            { EquipSlotIndex.Top, null }, 
            { EquipSlotIndex.Bottom, null }, 
            { EquipSlotIndex.Trinket1, null }, 
            { EquipSlotIndex.Trinket2, null },
        };


        public void Initialize(CharacterBehaviour cb)
        {
            this.cb = cb;
            Load();
        }

        public void TryArm(Equipment equipment)
        {
            // NullCheck
            if(equipment.IsNullOrDestroyed()) return;
            
            // Check AvailableClass
            if(!IsAvailableClass(equipment)) return;

            // Get Matched Slot
            var targetSlot = FindEquipSlot(equipment);
            
            TryDisarm(targetSlot);

            Table[targetSlot] = equipment;
            cb.StatTable.Add($"{equipment.EquipType}.{equipment.Title}", equipment.Spec);
        }

        public void TryDisarm(EquipSlotIndex slotIndex)
        {
            if (Table[slotIndex].IsNullOrEmpty()) return;

            var currentEquipment = Table[slotIndex];
            
            cb.StatTable.Remove($"{currentEquipment.EquipType}.{slotIndex}", currentEquipment.Spec);
            Table[slotIndex] = null;
        }

        public void Save()
        {
            var infoTable = new Dictionary<EquipSlotIndex, EquipmentInfo>();
            Table.ForEach(table => infoTable.Add(table.Key, table.Value.Info));
            
            SaveManager.Save($"{cb.Name}'s Equipments", infoTable);
        }
        
        public void Load()
        {
            var infoTable = SaveManager.Load<Dictionary<EquipSlotIndex, EquipmentInfo>>($"{cb.Name}'s Equipments");

            if (infoTable.IsNullOrEmpty())
            {
                TryArm(EquipmentInfo.CreateEquipment(weaponInfo, transform));
                TryArm(EquipmentInfo.CreateEquipment(headInfo, transform));
                TryArm(EquipmentInfo.CreateEquipment(topInfo, transform));
                TryArm(EquipmentInfo.CreateEquipment(bottomInfo, transform));
                TryArm(EquipmentInfo.CreateEquipment(trinket1Info, transform));
                TryArm(EquipmentInfo.CreateEquipment(trinket2Info, transform));
            }
            else
            {
                infoTable.ForEach(info => TryArm(EquipmentInfo.CreateEquipment(info.Value, transform)));
            }
        }
        

        private bool IsAvailableClass(Equipment equipment)
        {
            if(equipment.AvailableClass.HasFlag(cb.CombatClass)) return true;
            
            Debug.LogWarning($"Not Available. "
                             + $"Available Class:{equipment.AvailableClass}. Target Class:{cb.CombatClass}");
            return false;
        }
        
        private EquipSlotIndex FindEquipSlot(Equipment equipment)
        {
            var targetSlot = equipment.EquipType switch
            {
                EquipType.Weapon => EquipSlotIndex.Weapon,
                EquipType.Head   => EquipSlotIndex.Head,
                EquipType.Top    => EquipSlotIndex.Top,
                EquipType.Bottom => EquipSlotIndex.Bottom,
                EquipType.Trinket => Table[EquipSlotIndex.Trinket2].IsNullOrEmpty()
                    ? EquipSlotIndex.Trinket2
                    : EquipSlotIndex.Trinket1,
                EquipType.None => throw new ArgumentOutOfRangeException(),
                _              => throw new ArgumentOutOfRangeException()
            };

            return targetSlot;
        }
    }
}
