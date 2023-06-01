using System.Collections.Generic;
using Common.Equipments;
using UnityEngine;

namespace Common
{
    public static class EquipmentUtility
    {
        public static void LoadInstance(DataIndex dataIndex, EquipmentEntity entity)
        {
            entity.DataIndex = dataIndex;
            entity.ItemName  = dataIndex.ToString().DivideWords();
            entity.Icon      = Database.EquipmentSpriteData.Get(dataIndex);
            entity.EquipType = (EquipType)dataIndex.GetCategory();
            entity.Tier      = dataIndex.GetNumberOfDataIndex(3);
            
            Upgrade(entity.UpgradeLevel, entity);
        }
        
        public static void Upgrade(int level, EquipmentEntity entity)
        {
            var equipmentKey = entity.EquipType.ToString();

            if (level is < 1 or > 18)
            {
                Debug.LogError($"Upgrade Level Error. Input:{level}");
                return;
            }

            var arrayIndex = level - 1;
            entity.ConstStatSpec.Clear();
            
            switch (entity.EquipType)
            {
                case EquipType.Weapon:
                {
                    var weaponData = Database.WeaponData(entity.DataIndex);
            
                    entity.ConstStatSpec.Add(StatType.MinDamage, equipmentKey, weaponData.MinDamage[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.MaxDamage, equipmentKey, weaponData.MaxDamage[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, weaponData.Power[arrayIndex]);
                    break;
                }
                case EquipType.Head:
                {
                    var headData = Database.HeadData(entity.DataIndex);
                    
                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, headData.Power[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Health, equipmentKey, headData.Health[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, headData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Top:
                {
                    var topData = Database.TopData(entity.DataIndex);

                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, topData.Power[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Health, equipmentKey, topData.Health[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, topData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Glove:
                {
                    var gloveData = Database.GloveData(entity.DataIndex);

                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, gloveData.Power[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Health, equipmentKey, gloveData.Health[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, gloveData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Bottom:
                {
                    var bottomData = Database.BottomData(entity.DataIndex);

                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, bottomData.Power[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Health, equipmentKey, bottomData.Health[arrayIndex]);
                    entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, bottomData.Armor[arrayIndex]);
                    break;
                }
                default:
                {
                    Debug.LogWarning($"Unvalidated DataIndex in. Input:{entity.DataIndex}");
                    break;
                }
            }
        }
        
        public static void ImportEquipment(DataIndex dataIndex, EquipmentEntity entity)
        {
            entity.DataIndex = dataIndex;
            entity.ItemName = dataIndex.ToString().DivideWords();
            entity.Icon = Database.EquipmentSpriteData.Get(dataIndex);
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
        public static List<Ingredient> RequiredMaterialsForUpgrade(int toUpgrade)
        {
            var result = new List<Ingredient>();
            // var tier = Mathf.Min(toUpgrade / 6 + 1, 3);
            // var mod = toUpgrade % 6;
            // // var themeIndex = relicType.GetTheme().GetThemeIndex(); // 01 ~ 06
            // var main = (MaterialType)((int)DataIndex.Material * 1000000 + 1 * 10000 + tier);
            // var sub = main.GetNextTheme();
            // var extra = sub.GetNextTheme();
            //
            // switch (mod)
            // {
            //     case 0 :
            //     {
            //         result.Add(new Ingredient(main, 1)); 
            //         break;
            //     }
            //     case 1:
            //     {
            //         result.Add(new Ingredient(main, 2));
            //         result.Add(new Ingredient(sub, 1)); 
            //         break;
            //     }
            //     case 2:
            //     {
            //         result.Add(new Ingredient(main, 3));
            //         result.Add(new Ingredient(sub, 2)); 
            //         result.Add(new Ingredient(extra, 1));
            //         break;
            //     }
            //     case 3:
            //     {
            //         result.Add(new Ingredient(main, 4));
            //         result.Add(new Ingredient(sub, 3)); 
            //         result.Add(new Ingredient(extra, 2));
            //         break;
            //     }
            //     case 4:
            //     {
            //         result.Add(new Ingredient(main, 5));
            //         result.Add(new Ingredient(sub, 4)); 
            //         result.Add(new Ingredient(extra, 3));
            //         break;
            //     }
            //     case 5:
            //     {
            //         result.Add(new Ingredient(main, 6));
            //         result.Add(new Ingredient(sub, 5)); 
            //         result.Add(new Ingredient(extra, 4));
            //         break;
            //     }
            // }
        
            return result;
        }
    }
}
