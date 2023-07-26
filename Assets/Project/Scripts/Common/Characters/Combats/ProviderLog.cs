using System.Collections.Generic;
using System.Linq;

namespace Common.Characters.Combats
{
    public class ProviderLog
    {
        public int HitCount { get; private set; }
        public int CriticalHitCount { get; private set; }
        public float TotalDamage { get; private set; }
        public float TotalHeal { get; private set; }
        public List<CombatLog> this[DataIndex combatIndex] => CombatLogTable[combatIndex];

        /// <summary>
        /// Key is Provider's Skill, StatusEffect and so on
        /// </summary>
        private Table<DataIndex, List<CombatLog>> CombatLogTable { get; } = new();
        
        
        public void Initialize(ActionTable<CombatLog> combatProvidedAction)
        {
            combatProvidedAction.Add("BattleLog", Add);
        }
        
        public void Dispose()
        {
            CombatLogTable.Clear();
        }

        public void Add(CombatLog log)
        {
            if (!CombatLogTable.ContainsKey(log.CombatIndex))
            {
                CombatLogTable.Add(log.CombatIndex, new List<CombatLog>());
            }
            
            CombatLogTable[log.CombatIndex].Add(log);

            switch (log.Type)
            {
                case CombatEntityType.Damage:
                {
                    TotalDamage += log.Value;
                    HitCount++;
                    break;
                }
                case CombatEntityType.CriticalDamage:
                {
                    TotalDamage += log.Value;
                    HitCount++;
                    CriticalHitCount++;
                    break;
                }
                case CombatEntityType.Heal:
                {
                    TotalHeal += log.Value;
                    HitCount++;
                    break;
                }
                case CombatEntityType.CriticalHeal:
                {
                    TotalHeal += log.Value;
                    HitCount++;
                    CriticalHitCount++;
                    break;
                }
                // case CombatEntityType.Evade:          break;
                // case CombatEntityType.Block:          break;
                // case CombatEntityType.Absorb:         break;
                // case CombatEntityType.Immune:         break;
                default: return;
            }
        }

        public float GetSkillTotalValues(DataIndex combatIndex) => CombatLogTable[combatIndex].Sum(log => log.Value);
        public float GetSkillMinValue(DataIndex combatIndex) => CombatLogTable[combatIndex].Min(log => log.Value);
        public float GetSkillMaxValue(DataIndex combatIndex) => CombatLogTable[combatIndex].Max(log => log.Value);
        public float GetSkillAverageValue(DataIndex combatIndex) => CombatLogTable[combatIndex].Average(log => log.Value);
    }
}
