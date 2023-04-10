using System;
using System.Collections.Generic;
using Common.Equipments;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterEquipment : MonoBehaviour
    {
        #region TEMP
        [SerializeField] private EquipmentInfo weaponInfo;
        [SerializeField] private EquipmentInfo headInfo;
        [SerializeField] private EquipmentInfo topInfo;
        [SerializeField] private EquipmentInfo bottomInfo;
        [SerializeField] private EquipmentInfo trinket1Info;
        [SerializeField] private EquipmentInfo trinket2Info;
        #endregion

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private Dictionary<EquipSlotIndex, Equipment> equipmentTable { get; } = new(6)
        {
            { EquipSlotIndex.Weapon, null }, 
            { EquipSlotIndex.Head, null }, 
            { EquipSlotIndex.Top, null }, 
            { EquipSlotIndex.Bottom, null }, 
            { EquipSlotIndex.Trinket1, null }, 
            { EquipSlotIndex.Trinket2, null },
        };
        

        public void TryArm(Equipment equipment)
        {
            // NullCheck
            if(equipment.IsNullOrDestroyed()) return;
            
            // Check AvailableClass
            if(!IsAvailableClass(equipment)) return;

            // Get Matched Slot
            var targetSlot = FindEquipSlot(equipment);
            
            TryDisarm(targetSlot);

            equipmentTable[targetSlot] = equipment;
            Cb.StatTable.Add($"{equipment.EquipType}.{targetSlot}", equipment.Spec);
        }

        public void TryDisarm(EquipSlotIndex slotIndex)
        {
            if (equipmentTable[slotIndex].IsNullOrEmpty()) return;

            var currentEquipment = equipmentTable[slotIndex];
            
            Cb.StatTable.Remove($"{currentEquipment.EquipType}.{slotIndex}", currentEquipment.Spec);
            equipmentTable[slotIndex] = null;
        }

        public void Save()
        {
#if !UNITY_EDITOR
            var infoTable = new Dictionary<EquipSlotIndex, EquipmentInfo>();
            equipmentTable.ForEach(table => infoTable.Add(table.Key, table.Value.Info));
            MainManager.Save.Save($"{Cb.Name}'s Equipments", infoTable);
#endif
        }
        
        public void Load()
        {
#if UNITY_EDITOR
            TryArm(EquipmentInfo.CreateEquipment(weaponInfo, transform));
            TryArm(EquipmentInfo.CreateEquipment(headInfo, transform));
            TryArm(EquipmentInfo.CreateEquipment(topInfo, transform));
            TryArm(EquipmentInfo.CreateEquipment(bottomInfo, transform));
            TryArm(EquipmentInfo.CreateEquipment(trinket1Info, transform));
            TryArm(EquipmentInfo.CreateEquipment(trinket2Info, transform));
#else
            var infoTable = MainManager.Save.Load<Dictionary<EquipSlotIndex, EquipmentInfo>>($"{Cb.Name}'s Equipments");
            infoTable.ForEach(info => TryArm(Generate(info.Value)));
#endif
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
                EquipType.Trinket => equipmentTable[EquipSlotIndex.Trinket2].IsNullOrEmpty()
                    ? EquipSlotIndex.Trinket2
                    : EquipSlotIndex.Trinket1,
                EquipType.None => throw new ArgumentOutOfRangeException(),
                _              => throw new ArgumentOutOfRangeException()
            };

            return targetSlot;
        }

        private void Awake()
        {
            Load();
        }
    }
}
