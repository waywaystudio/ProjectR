using System;
using System.Collections.Generic;
using Common.Equipments;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterEquipment : MonoBehaviour
    {
        [SerializeField] private CharacterData data;
        
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
            data.EquipmentTable.ForEach(dataTable =>
            {
                Table[dataTable.Key] = EquipmentInfo.CreateEquipment(dataTable.Value, transform);
                Cb.StatTable.Add($"{Table[dataTable.Key].EquipType}.{Table[dataTable.Key].Title}", Table[dataTable.Key].Spec);
            });

            if (Table[EquipSlotIndex.Weapon] == null && Table[EquipSlotIndex.Top] == null)
            {
                Equip(EquipmentInfo.CreateEquipment(weaponInfo, transform), out _);
                Equip(EquipmentInfo.CreateEquipment(topInfo, transform), out _);
            }
        }

        public void Equip(Equipment equipment, out Equipment disarmed)
        {
            if (equipment == null)
            {
                disarmed = null;
                return;
            }
            
            var targetSlot = FindEquipSlot(equipment);

            TryDisarm(targetSlot, out disarmed);
            
            if (!TryArm(targetSlot, equipment)) return;
            
            equipment.transform.SetParent(transform);
        }

        public bool TryDisarm(EquipSlotIndex slotIndex, out Equipment disarmed)
        {
            disarmed = Table[slotIndex];

            if (disarmed == null) return false;

            Table[slotIndex]      = null;
            
            data.EquipmentTable.TryRemove(slotIndex);
            Cb.StatTable.Remove($"{disarmed.EquipType}.{disarmed.Title}", disarmed.Spec);

            return true;
        }
        

        private bool TryArm(EquipSlotIndex slotIndex, Equipment equipment)
        {
            // Check AvailableClass
            if(!IsAvailableClass(equipment)) return false;

            Table.TryAdd(slotIndex, equipment, true);
            data.EquipmentTable.TryAdd(slotIndex, equipment.Info, true);
            Cb.StatTable.Add($"{equipment.EquipType}.{equipment.Title}", equipment.Spec);

            return true;
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
    }
}
