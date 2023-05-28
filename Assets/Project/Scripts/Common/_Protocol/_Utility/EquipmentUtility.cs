using System.Collections.Generic;
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
            entity.ConstStatSpec.Clear();
            
            var equipmentKey = entity.EquipType.ToString();

            switch (dataIndex.GetCategory())
            {
                case DataIndex.Weapon:
                {
                    var weaponData = Database.WeaponData(dataIndex);
            
                    (weaponData.MinDamage != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.MinDamage, equipmentKey, weaponData.MinDamage));
                    (weaponData.MaxDamage != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.MaxDamage, equipmentKey, weaponData.MaxDamage));
                    (weaponData.Power != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Power, equipmentKey, weaponData.Power));
                    break;
                }
                case DataIndex.Head:
                {
                    var headData = Database.HeadData(dataIndex);

                    (headData.Power  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Power, equipmentKey, headData.Power));
                    (headData.Health != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Health, equipmentKey, headData.Health));
                    (headData.Armor  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, headData.Armor));
                    break;
                }
                case DataIndex.Top:
                {
                    var topData = Database.TopData(dataIndex);

                    (topData.Power  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Power, equipmentKey, topData.Power));
                    (topData.Health != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Health, equipmentKey, topData.Health));
                    (topData.Armor  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, topData.Armor));
                    break;
                }
                case DataIndex.Glove:
                {
                    var gloveData = Database.GloveData(dataIndex);

                    (gloveData.Power  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Power, equipmentKey, gloveData.Power));
                    (gloveData.Health != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Health, equipmentKey, gloveData.Health));
                    (gloveData.Armor  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, gloveData.Armor));
                    break;
                }
                case DataIndex.Bottom:
                {
                    var bottomData = Database.BottomData(dataIndex);

                    (bottomData.Power  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Power, equipmentKey, bottomData.Power));
                    (bottomData.Health != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Health, equipmentKey, bottomData.Health));
                    (bottomData.Armor  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, bottomData.Armor));
                    break;
                }
                case DataIndex.Boot:
                {
                    var bootData = Database.BootData(dataIndex);
            
                    (bootData.Power  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Power, equipmentKey, bootData.Power));
                    (bootData.Health != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Health, equipmentKey, bootData.Health));
                    (bootData.Armor  != 0.0f).OnTrue(() => entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, bootData.Armor));
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

        /// <summary>
        /// Vice Stat Enchant of Equipment 
        /// </summary>
        public static void EnchantRelic(EthosType ethosType, int tier)
        {
            // relic Type에 따라서 필수 vice 1개 선택
            // Random으로 2개를 선택. 이 과정에서 필수로 선택된 vice는 제외 // 따라서 반드시 총 3개의 vice가 있음.
            // 3개의 Vice가 tier * 6 값을 나누어 먹음.
        }
            
        // TODO. MaterialType과 int를 가지고 있는 클래스가 필요할 듯 하다.
        public static List<Ingredient> RequiredMaterialsForUpgrade(RelicType relicType, int toUpgrade)
        {
            var result = new List<Ingredient>();
            var tier = Mathf.Min(toUpgrade / 6 + 1, 3);
            var mod = toUpgrade % 6;
            var themeIndex = relicType.GetTheme().GetThemeIndex(); // 01 ~ 06
            var main = (MaterialType)((int)DataIndex.Material * 1000000 + themeIndex * 10000 + tier);
            var sub = main.GetNextTheme();
            var extra = sub.GetNextTheme();
            
            

            switch (mod)
            {
                case 0 :
                {
                    result.Add(new Ingredient(main, 1)); 
                    break;
                }
                case 1:
                {
                    result.Add(new Ingredient(main, 2));
                    result.Add(new Ingredient(sub, 1)); 
                    break;
                }
                case 2:
                {
                    result.Add(new Ingredient(main, 3));
                    result.Add(new Ingredient(sub, 2)); 
                    result.Add(new Ingredient(extra, 1));
                    break;
                }
                case 3:
                {
                    result.Add(new Ingredient(main, 4));
                    result.Add(new Ingredient(sub, 3)); 
                    result.Add(new Ingredient(extra, 2));
                    break;
                }
                case 4:
                {
                    result.Add(new Ingredient(main, 5));
                    result.Add(new Ingredient(sub, 4)); 
                    result.Add(new Ingredient(extra, 3));
                    break;
                }
                case 5:
                {
                    result.Add(new Ingredient(main, 6));
                    result.Add(new Ingredient(sub, 5)); 
                    result.Add(new Ingredient(extra, 4));
                    break;
                }
            }
        
            return result;
        }
    }
}
