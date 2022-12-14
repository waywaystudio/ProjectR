using System;
using MainGame;
using Sirenix.OdinInspector;

namespace Common.Character.Data
{
    [Serializable]
    public class EquipmentItem
    {
        public string ItemName;
        public int ID;
        
        public double Hp;
        public float Critical;
        public float Haste;
        public float Hit;
        public float Evade;

        [Button]
        public void SetStats() => SetStats(ItemName);
        public void SetStats(string itemName)
        {
            var equipmentData = MainData.GetEquipmentData(itemName);
            
            Hp = equipmentData.HP;
            Critical = equipmentData.Critical;
            Haste = equipmentData.Haste;
            Hit = equipmentData.Hit;
            Evade = equipmentData.Evade;
        }
    }
}
