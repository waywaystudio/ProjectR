using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Common
{
    [Serializable]
    public class RelicEntity
    {
        [SerializeField] private bool isUnlocked;
        [SerializeField] private StatSpec statSpec;
        [SerializeField] private EthosSpec ethosSpec;

        public RelicEntity() { }
        public RelicEntity(RelicType type)
        {
            isUnlocked = false;
            ethosSpec  = new EthosSpec(EthosType.None, "Relic", 0);
            statSpec   = new StatSpec();
        }
        public RelicEntity(RelicType type, StatType stat)
        {
            isUnlocked = false;
            ethosSpec      = new EthosSpec(EthosType.None, "Relic",0);
            statSpec   = new StatSpec();
            statSpec.Add(stat, "Relic", 0.1f);
        }
            
        public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
        public EthosSpec EthosSpec { get => ethosSpec; set => ethosSpec = value; }
        public StatSpec StatSpec { get => statSpec; set => statSpec = value; }

        public void Conversion(int tier, string equipKey)
        {
            // TODO. Relic의 Conversion 기획.
            // 임시 기획 : Random Secondary Stat이 3개.
            
            StatSpec.Clear();
            
            var rand = new Random();
            var array = (StatType[])Enum.GetValues(typeof(StatType));
            var arrayCount = array.Length;
            var conversionCount = 3;
            
            // Shuffle
            while (arrayCount > 1)
            {
                arrayCount--;
                var k = rand.Next(arrayCount + 1);
                (array[k], array[arrayCount]) = (array[arrayCount], array[k]);
            }
            
            // Conversion
            foreach (var statType in array)
            {
                if (conversionCount == 0) break;
                if (!statType.IsSecondary()) continue;
                
                StatSpec.Add(statType, equipKey, UnityEngine.Random.Range(0.01f * tier, 0.02f * tier));
                conversionCount--;
            }
        }

        public void Enchant(int tier, string equipKey)
        {
            // TODO. Relic의 Enchant 기획. 
            // 임시 기획 : 관련된 Ethos의 Vice가 반드시 1개.
            
            ethosSpec.Clear();
            
            var rand = new Random();
            var array = (EthosType[])Enum.GetValues(typeof(EthosType));
            var arrayCount = array.Length;
            var enchantCount = 3;
            
            // Shuffle
            while (arrayCount > 1)
            {
                arrayCount--;
                var k = rand.Next(arrayCount + 1);
                (array[k], array[arrayCount]) = (array[arrayCount], array[k]);
            }
            
            // Enchant
            foreach (var ethosType in array)
            {
                if (enchantCount == 0) break;
                if (!ethosType.IsVice() || ethosType == EthosType.None) continue;
                
                ethosSpec.Add(ethosType, equipKey, UnityEngine.Random.Range(tier * 1, tier * 4));
                enchantCount--;
            }
        }
    }
    
    public class RelicTable
    {
        public Dictionary<RelicType, RelicEntity> Table { get; set; }

        public RelicType CurrentRelicType { get; private set; } = RelicType.None;
        public EthosSpec RelicEthos => GetRelic(CurrentRelicType)?.EthosSpec;
        public StatSpec EnchantSpec => GetRelic(CurrentRelicType)?.StatSpec;

        public bool IsUnlocked(RelicType type) => Table.ContainsKey(type) && Table[type].IsUnlocked;
        public void UnlockRelic(RelicType type)
        {
            if (IsUnlocked(type)) return;

            Table.TryAdd(type, new RelicEntity());
            Table[type].IsUnlocked = true;
        }
        
        public void ChangeTo(RelicType type) => CurrentRelicType = type;

        public void Enchant(RelicType type, int tier, string equipKey)
        {
            var relicEntity = GetRelic(type);
            
            relicEntity.Enchant(tier, $"{equipKey}.Enchant");
        }

        public void Conversion(RelicType type, int tier, string equipKey)
        {
            var relicEntity = GetRelic(type);
            
            relicEntity.Conversion(tier, $"{equipKey}.Conversion");
        }
        
        public RelicEntity GetRelic(RelicType type) => Table.TryGetValue(type, out var value) 
            ? value 
            : null;


        public RelicTable()
        {
            Table = new Dictionary<RelicType, RelicEntity>
            {
                { RelicType.Vowed, new RelicEntity(RelicType.Vowed, StatType.Armor) },
                { RelicType.Valorous, new RelicEntity(RelicType.Valorous, StatType.Power) },
                { RelicType.Veterans, new RelicEntity(RelicType.Veterans, StatType.CriticalChance) },
                { RelicType.Vital, new RelicEntity(RelicType.Vital, StatType.Mastery) },
                { RelicType.Verdicts, new RelicEntity(RelicType.Verdicts, StatType.Retention) },
                { RelicType.Votive, new RelicEntity(RelicType.Votive, StatType.MoveSpeed) },
            };
        }
    }
}
