using System;
using System.Collections.Generic;
using Common.Equipments;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters
{
    [Serializable]
    public class CharacterEquipmentEntity
    {
        private List<EquipmentInfo> allEquipment = new();
        
        [ShowInInspector] public EquipmentInfo WeaponInfo { get; set; }
        [ShowInInspector] public EquipmentInfo HeadInfo { get; set; }
        [ShowInInspector] public EquipmentInfo TopInfo { get; set; }
        [ShowInInspector] public EquipmentInfo GloveInfo { get; set; }
        [ShowInInspector] public EquipmentInfo BottomInfo { get; set; }
        [ShowInInspector] public EquipmentInfo BootInfo { get; set; }

        public List<EquipmentInfo> AllEquipment
        {
            get
            {
                if (allEquipment.IsNullOrEmpty())
                {
                    allEquipment.Add(WeaponInfo);
                    allEquipment.Add(HeadInfo);
                    allEquipment.Add(TopInfo);
                    allEquipment.Add(GloveInfo);
                    allEquipment.Add(BottomInfo);
                    allEquipment.Add(BootInfo);
                }

                return allEquipment;
            }
        }

        // Enchanter Save Data
        // DataIndex
        // Forms * 6 (Vice Form)
            // Upgrade Level
            // Attached Secondary Stat : Primary Stat is Const.
            // Vice Stat
            //
    }
}
