using System;
using System.Collections.Generic;
using Common.Equipments;
using UnityEngine;

namespace Common.Characters
{
    [Serializable]
    public class CharacterSerializeEntity
    {
        [SerializeField] private List<EquipmentInfo> equipmentInfoList;

        public List<EquipmentInfo> EquipmentInfoList => equipmentInfoList;
        public EquipmentInfo TryGetEquipment(EquipType type)
            => EquipmentInfoList.TryGetElement(info => info.EquipType == type);


        // Enchanter Save Data
        // DataIndex
        // Forms * 6 (Vice Form)
            // Upgrade Level
            // Attached Secondary Stat : Primary Stat is Const.
            // Vice Stat
            // 
        
        
            
        public void Save()
        {
            
        }

        public void Load()
        {
            
        }
    }
}
