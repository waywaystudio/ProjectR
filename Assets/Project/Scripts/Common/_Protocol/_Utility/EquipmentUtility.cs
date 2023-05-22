using Common.Equipments;
using UnityEngine;

namespace Common
{
    public static class EquipmentUtility
    {
        public static void ImportEquipment(DataIndex dataIndex, EquipmentEntity entity)
        {
            entity.DataIndex = dataIndex;
            entity.ItemName = dataIndex.ToString().DivideWords();
            entity.Icon = Database.EquipmentSpriteData.Get(dataIndex);
            entity.AvailableClassType = CombatClassType.All; // TEMP
            entity.ConstSpec.Clear();
            
            var equipmentKey = entity.EquipType.ToString();

            switch (dataIndex.GetCategory())
            {
                case DataIndex.Weapon:
                {
                    var weaponData = Database.WeaponData(dataIndex);
            
                    (weaponData.MinDamage != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.MinDamage, equipmentKey, weaponData.MinDamage));
                    (weaponData.MaxDamage != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.MaxDamage, equipmentKey, weaponData.MaxDamage));
                    (weaponData.Power != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Power, equipmentKey, weaponData.Power));
                    break;
                }
                case DataIndex.Head:
                {
                    var headData = Database.HeadData(dataIndex);

                    (headData.Power  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Power, equipmentKey, headData.Power));
                    (headData.Health != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Health, equipmentKey, headData.Health));
                    (headData.Armor  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Armor, equipmentKey, headData.Armor));
                    break;
                }
                case DataIndex.Top:
                {
                    var topData = Database.TopData(dataIndex);

                    (topData.Power  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Power, equipmentKey, topData.Power));
                    (topData.Health != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Health, equipmentKey, topData.Health));
                    (topData.Armor  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Armor, equipmentKey, topData.Armor));
                    break;
                }
                case DataIndex.Glove:
                {
                    var gloveData = Database.GloveData(dataIndex);

                    (gloveData.Power  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Power, equipmentKey, gloveData.Power));
                    (gloveData.Health != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Health, equipmentKey, gloveData.Health));
                    (gloveData.Armor  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Armor, equipmentKey, gloveData.Armor));
                    break;
                }
                case DataIndex.Bottom:
                {
                    var bottomData = Database.BottomData(dataIndex);

                    (bottomData.Power  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Power, equipmentKey, bottomData.Power));
                    (bottomData.Health != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Health, equipmentKey, bottomData.Health));
                    (bottomData.Armor  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Armor, equipmentKey, bottomData.Armor));
                    break;
                }
                case DataIndex.Boot:
                {
                    var bootData = Database.BootData(dataIndex);
            
                    (bootData.Power  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Power, equipmentKey, bootData.Power));
                    (bootData.Health != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Health, equipmentKey, bootData.Health));
                    (bootData.Armor  != 0.0f).OnTrue(() => entity.ConstSpec.Add(StatType.Armor, equipmentKey, bootData.Armor));
                    break;
                }
                case DataIndex.Trinket:
                {
                    Debug.Log($"Trinket Equipment is not support yet. Input:{dataIndex}");
                    break;
                }
                default:
                {
                    Debug.LogWarning($"Unvalidated DataIndex in. Input:{dataIndex}");
                    break;
                }
            }
        }
    }
}
