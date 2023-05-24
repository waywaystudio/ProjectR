using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class RelicTable
    {
        public Dictionary<RelicType, RelicEntity> Table { get; set; }

        public RelicType CurrentRelicType { get; private set; } = RelicType.None;
        public EthosSpec RelicEthos => GetRelic(CurrentRelicType)?.Ethos;
        public StatSpec EnchantSpec => GetRelic(CurrentRelicType)?.StatSpec;

        public bool IsUnlocked(RelicType type) => Table.ContainsKey(type) && Table[type].IsUnlocked;
        public void UnlockRelic(RelicType type)
        {
            if (IsUnlocked(type)) return;

            Table.TryAdd(type, new RelicEntity());
            Table[type].IsUnlocked = true;
        }
        
        public void ChangeTo(RelicType type) => CurrentRelicType = type;
        
        private RelicEntity GetRelic(RelicType type) => Table.TryGetValue(type, out var value) 
            ? value 
            : null;


        public RelicTable()
        {
            Table = new Dictionary<RelicType, RelicEntity>
            {
                { RelicType.Vow, new RelicEntity(RelicType.Vow, StatType.Armor) },
                { RelicType.Valor, new RelicEntity(RelicType.Valor, StatType.Power) },
                { RelicType.Veterans, new RelicEntity(RelicType.Veterans, StatType.CriticalChance) },
                { RelicType.Vital, new RelicEntity(RelicType.Vital, StatType.Mastery) },
                { RelicType.Verdicts, new RelicEntity(RelicType.Verdicts, StatType.Retention) },
                { RelicType.Votive, new RelicEntity(RelicType.Votive, StatType.MoveSpeed) },
            };
        }

        [Serializable]
        public class RelicEntity
        {
            [SerializeField] private bool isUnlocked;
            [SerializeField] private EthosSpec ethos;
            [SerializeField] private StatSpec statSpec;

            public RelicEntity() { }
            public RelicEntity(RelicType type)
            {
                isUnlocked = false;
                ethos      = new EthosSpec(type.ConvertToExcess(), "Relic",6);
                statSpec       = new StatSpec();
            }
            public RelicEntity(RelicType type, StatType stat)
            {
                isUnlocked = false;
                ethos      = new EthosSpec(type.ConvertToExcess(), "Relic",6);
                statSpec   = new StatSpec();
                statSpec.Add(stat, "Relic", 0.1f);
            }
            
            public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
            public EthosSpec Ethos { get => ethos; set => ethos = value; }
            public StatSpec StatSpec { get => statSpec; set => statSpec = value; }
        }
    }
}
