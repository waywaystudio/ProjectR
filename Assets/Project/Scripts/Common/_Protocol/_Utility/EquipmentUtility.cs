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
            entity.PrimeVice ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
            entity.SubVice ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
            entity.ExtraVice ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);

            SpecInitialize(entity);
        }

        public static void SpecInitialize(EquipmentEntity entity)
        {
            var equipmentKey = entity.EquipType.ToString();
            var arrayIndex = 0;
            
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

            var arrayIndex = entity.UpgradeLevel - 1;

            entity.UpgradeLevel++;
            
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
            
            // entity.PrimeVice    ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
            // entity.SubVice      ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
            // entity.ExtraVice    ??= new EthosEntity(EthosType.None, entity.EquipType.ToString(), 0);
            
            Upgrade(entity);
            Debug.Log($"Evolve To {entity.DataIndex.ToString()}");
        }
    }
}
