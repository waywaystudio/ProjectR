using System;
using Core;
using MainGame;

namespace Character.Data
{
    [Serializable]
    public class EquipmentItem
    {
        public string ItemName;
        public int ID;
        
        public float MaxHp;
        public float Critical;
        public float Haste;
        public float Hit;
        public float Evade;

        public void SetStats() => SetStats(ItemName);
        public void SetStats(string itemName)
        {
            var equipmentData = MainData.GetEquipment(itemName.ToEnum<IDCode>());
            
            MaxHp = equipmentData.HP;
            Critical = equipmentData.Critical;
            Haste = equipmentData.Haste;
            Hit = equipmentData.Hit;
            Evade = equipmentData.Evade;
        }
    }
}
