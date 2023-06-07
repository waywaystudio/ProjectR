using Common.Equipments;
using UnityEngine;

namespace Common
{
    public static class EquipmentUtility
    {
        public static string GetWeaponName(DataIndex dataIndex)
        {
            return Database.WeaponData(dataIndex).Name;
        }
        public static string GetArmorName(DataIndex dataIndex)
        {
            var category = (EquipType)dataIndex.GetCategory();
            
            return category switch
            {
                EquipType.Head => Database.HeadData(dataIndex).Name,
                EquipType.Top => Database.TopData(dataIndex).Name,
                EquipType.Glove => Database.GloveData(dataIndex).Name,
                EquipType.Bottom => Database.BottomData(dataIndex).Name,
                _ => "None",
            };
        }

        public static float GetWeaponStat(DataIndex dataIndex, StatType statType, int level)
        {
            var arrayIndex = level - 1;
            var weaponData = Database.WeaponData(dataIndex);
            
            float Error()
            {
                Debug.LogError($"Can't get {statType.ToString()} in Weapon");
                return 0f;
            }

            return statType switch
            {
                StatType.MinDamage => weaponData.MinDamage[arrayIndex],
                StatType.MaxDamage => weaponData.MaxDamage[arrayIndex],
                StatType.Power     => weaponData.Power[arrayIndex],
                _                  => Error(),
            };
        }
        public static float GetArmorStat(DataIndex dataIndex, StatType statType, int level)
        {
            var category = (EquipType)dataIndex.GetCategory();
            var arrayIndex = level - 1;

            float Error()
            {
                Debug.LogError($"Can't get {statType.ToString()} in {category.ToString()}");
                return 0f;
            }
            
            switch (category)
            {
                case EquipType.Head:
                {
                    var armorData = Database.HeadData(dataIndex);

                    return statType switch
                    {
                        StatType.Power  => armorData.Power[arrayIndex],
                        StatType.Health => armorData.Health[arrayIndex],
                        StatType.Armor  => armorData.Armor[arrayIndex],
                        _               => Error(),
                    };
                }
                case EquipType.Top:
                {
                    var armorData = Database.TopData(dataIndex);
                    
                    return statType switch
                    {
                        StatType.Power  => armorData.Power[arrayIndex],
                        StatType.Health => armorData.Health[arrayIndex],
                        StatType.Armor  => armorData.Armor[arrayIndex],
                        _               => Error(),
                    };
                }
                case EquipType.Glove:
                {
                    var armorData = Database.GloveData(dataIndex);
                    
                    return statType switch
                    {
                        StatType.Power  => armorData.Power[arrayIndex],
                        StatType.Health => armorData.Health[arrayIndex],
                        StatType.Armor  => armorData.Armor[arrayIndex],
                        _               => Error(),
                    };
                }
                case EquipType.Bottom:
                {
                    var armorData = Database.BottomData(dataIndex);
                    
                    return statType switch
                    {
                        StatType.Power  => armorData.Power[arrayIndex],
                        StatType.Health => armorData.Health[arrayIndex],
                        StatType.Armor  => armorData.Armor[arrayIndex],
                        _               => Error(),
                    };
                }
                default: return Error();
            }
        }


        public static void LoadInstance(DataIndex dataIndex, EquipmentEntity entity)
        {
            entity.DataIndex = dataIndex;
            entity.ItemName  = dataIndex.ToString().DivideWords();
            entity.Icon      = Database.EquipmentSpriteData.Get(dataIndex);
            entity.EquipType = (EquipType)dataIndex.GetCategory();
            entity.Tier      = dataIndex.GetNumberOfDataIndex(3);
            entity.PrimeVice ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
            entity.SubVice ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
            entity.ExtraVice ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);

            CreateSpec(entity);
            GenerateSpec(entity.UpgradeLevel, entity);
        }

        private static void CreateSpec(EquipmentEntity entity)
        {
            var equipmentKey = entity.EquipType.ToString();
            
            switch (entity.EquipType)
            {
                case EquipType.Weapon:
                {
                    var weaponData = Database.WeaponData(entity.DataIndex);
            
                    entity.ConstStatSpec.Add(StatType.MinDamage, equipmentKey, weaponData.MinDamage[0]);
                    entity.ConstStatSpec.Add(StatType.MaxDamage, equipmentKey, weaponData.MaxDamage[0]);
                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, weaponData.Power[0]);
                    break;
                }
                case EquipType.Head:
                {
                    var headData = Database.HeadData(entity.DataIndex);
                    
                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, headData.Power[0]);
                    entity.ConstStatSpec.Add(StatType.Health, equipmentKey, headData.Health[0]);
                    entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, headData.Armor[0]);
                    break;
                }
                case EquipType.Top:
                {
                    var topData = Database.TopData(entity.DataIndex);

                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, topData.Power[0]);
                    entity.ConstStatSpec.Add(StatType.Health, equipmentKey, topData.Health[0]);
                    entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, topData.Armor[0]);
                    break;
                }
                case EquipType.Glove:
                {
                    var gloveData = Database.GloveData(entity.DataIndex);

                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, gloveData.Power[0]);
                    entity.ConstStatSpec.Add(StatType.Health, equipmentKey, gloveData.Health[0]);
                    entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, gloveData.Armor[0]);
                    break;
                }
                case EquipType.Bottom:
                {
                    var bottomData = Database.BottomData(entity.DataIndex);

                    entity.ConstStatSpec.Add(StatType.Power, equipmentKey, bottomData.Power[0]);
                    entity.ConstStatSpec.Add(StatType.Health, equipmentKey, bottomData.Health[0]);
                    entity.ConstStatSpec.Add(StatType.Armor, equipmentKey, bottomData.Armor[0]);
                    break;
                }
                default:
                {
                    Debug.LogWarning($"Unvalidated DataIndex in. Input:{entity.DataIndex}");
                    break;
                }
            }
        }

        public static void GenerateSpec(int upgradeLevel, EquipmentEntity entity)
        {
            if (upgradeLevel is < 1 or > 6)
            {
                Debug.LogError($"Upgrade Level Error. Input:{upgradeLevel}");
                return;
            }

            var arrayIndex = upgradeLevel - 1;
            
            switch (entity.EquipType)
            {
                case EquipType.Weapon:
                {
                    var weaponData = Database.WeaponData(entity.DataIndex);

                    entity.ConstStatSpec.Change(StatType.MinDamage, weaponData.MinDamage[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.MaxDamage, weaponData.MaxDamage[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Power, weaponData.Power[arrayIndex]);
                    break;
                }
                case EquipType.Head:
                {
                    var headData = Database.HeadData(entity.DataIndex);
                    
                    entity.ConstStatSpec.Change(StatType.Power, headData.Power[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Health, headData.Health[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Armor, headData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Top:
                {
                    var topData = Database.TopData(entity.DataIndex);

                    entity.ConstStatSpec.Change(StatType.Power, topData.Power[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Health, topData.Health[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Armor, topData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Glove:
                {
                    var gloveData = Database.GloveData(entity.DataIndex);

                    entity.ConstStatSpec.Change(StatType.Power, gloveData.Power[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Health, gloveData.Health[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Armor, gloveData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Bottom:
                {
                    var bottomData = Database.BottomData(entity.DataIndex);

                    entity.ConstStatSpec.Change(StatType.Power, bottomData.Power[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Health, bottomData.Health[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Armor, bottomData.Armor[arrayIndex]);
                    break;
                }
                default:
                {
                    Debug.LogWarning($"Unvalidated DataIndex in. Input:{entity.DataIndex}");
                    break;
                }
            }
        }
        
        public static void Upgrade(EquipmentEntity entity)
        {
            if (entity.UpgradeLevel < 1)
            {
                Debug.LogError($"Upgrade Level Error. Input:{entity.UpgradeLevel}");
                return;
            }

            if (entity.UpgradeLevel > 6)
            {
                Evolve(entity);
                return;
            }

            entity.UpgradeLevel++;
            var arrayIndex = entity.UpgradeLevel - 1;

            switch (entity.EquipType)
            {
                case EquipType.Weapon:
                {
                    var weaponData = Database.WeaponData(entity.DataIndex);

                    entity.ConstStatSpec.Change(StatType.MinDamage, weaponData.MinDamage[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.MaxDamage, weaponData.MaxDamage[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Power, weaponData.Power[arrayIndex]);
                    break;
                }
                case EquipType.Head:
                {
                    var headData = Database.HeadData(entity.DataIndex);
                    
                    entity.ConstStatSpec.Change(StatType.Power, headData.Power[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Health, headData.Health[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Armor, headData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Top:
                {
                    var topData = Database.TopData(entity.DataIndex);

                    entity.ConstStatSpec.Change(StatType.Power, topData.Power[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Health, topData.Health[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Armor, topData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Glove:
                {
                    var gloveData = Database.GloveData(entity.DataIndex);

                    entity.ConstStatSpec.Change(StatType.Power, gloveData.Power[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Health, gloveData.Health[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Armor, gloveData.Armor[arrayIndex]);
                    break;
                }
                case EquipType.Bottom:
                {
                    var bottomData = Database.BottomData(entity.DataIndex);

                    entity.ConstStatSpec.Change(StatType.Power, bottomData.Power[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Health, bottomData.Health[arrayIndex]);
                    entity.ConstStatSpec.Change(StatType.Armor, bottomData.Armor[arrayIndex]);
                    break;
                }
                default:
                {
                    Debug.LogWarning($"Unvalidated DataIndex in. Input:{entity.DataIndex}");
                    break;
                }
            }
        }

        public static void Evolve(EquipmentEntity entity)
        {
            var tier = entity.Tier;

            if (tier >= 3)
            {
                Debug.LogWarning($"No more Evolve. Current Tier:{tier}");
                return;
            }
            
            entity.DataIndex    +=  101;
            entity.Tier++;
            entity.UpgradeLevel =   1;
            entity.ItemName     =   entity.DataIndex.ToString().DivideWords();
            entity.Icon         =   Database.EquipmentSpriteData.Get(entity.DataIndex);

            switch (entity.Tier)
            {
                case 2: entity.SubVice   ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
                    break;
                case 3: entity.ExtraVice ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
                    break;
            }

            Upgrade(entity);
        }
    }
}
