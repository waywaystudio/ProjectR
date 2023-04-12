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
        [SerializeField] private EquipmentInfo topInfo;

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>(true);

        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<EquipSlotIndex, Equipment> Table = new(6)
        {
            { EquipSlotIndex.Weapon, null },
            { EquipSlotIndex.Head, null },
            { EquipSlotIndex.Top, null },
            { EquipSlotIndex.Bottom, null },
            { EquipSlotIndex.Trinket1, null },
            { EquipSlotIndex.Trinket2, null },
        };


        public void Initialize()
        {
            Load();
        }

        public void Equip(Equipment equipment, out Equipment disarmed)
        {
            var targetSlot = FindEquipSlot(equipment);

            Disarm(targetSlot, out disarmed);
            Arm(targetSlot, equipment);
            
            equipment.transform.SetParent(transform);
        }


        private void Arm(EquipSlotIndex slotIndex, Equipment equipment)
        {
            // NullCheck
            if(equipment.IsNullOrDestroyed()) return;
            
            // Check AvailableClass
            if(!IsAvailableClass(equipment)) return;
            
            Debug.Log("Arm In");

            Table.TryAdd(slotIndex, equipment);
            Cb.StatTable.Add($"{equipment.EquipType}.{equipment.Title}", equipment.Spec);
        }

        private void Disarm(EquipSlotIndex slotIndex, out Equipment disarmed)
        {
            disarmed = Table[slotIndex];

            if (disarmed == null) return;
            
            disarmed = Table[slotIndex];
            
            Table[slotIndex] = null;
            Cb.StatTable.Remove($"{disarmed.EquipType}.{disarmed.Title}", disarmed.Spec);
        }

        private bool IsAvailableClass(Equipment equipment)
        {
            if(equipment.AvailableClass.HasFlag(Cb.CombatClass)) return true;
            
            Debug.LogWarning($"Not Available. "
                             + $"Available Class:{equipment.AvailableClass}. Target Class:{Cb.CombatClass}");
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
                EquipType.Trinket => Table[EquipSlotIndex.Trinket1] != null 
                    ? EquipSlotIndex.Trinket1
                    : EquipSlotIndex.Trinket2,
                EquipType.None => throw new ArgumentOutOfRangeException(),
                _              => throw new ArgumentOutOfRangeException()
            };

            return targetSlot;
        }
        
        
        public void Save()
        {
            var infoTable = new Dictionary<EquipSlotIndex, EquipmentInfo>();
            
            Table.ForEach(table =>
            {
                if (table.Value is null) return;
                infoTable.Add(table.Key, table.Value.Info);
            });
            
            SaveManager.Save($"{Cb.Name}'s Equipments", infoTable);
        }
        
        public void Load()
        {
            var infoTable = SaveManager.Load<Dictionary<EquipSlotIndex, EquipmentInfo>>($"{Cb.Name}'s Equipments");

            infoTable?.ForEach(info =>
            {
                Equip(EquipmentInfo.CreateEquipment(info.Value, transform), out _);
            });
        }
    }
}
