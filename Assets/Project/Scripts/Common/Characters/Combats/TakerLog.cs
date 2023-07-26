using System.Collections.Generic;

namespace Common.Characters.Combats
{
    public class TakerLog
    {
        public int BeatCount { get; private set; }
        public int BeCriticalHitCount { get; private set; }
        public float TotalDamaged { get; private set; }
        public float TotalHealed { get; private set; }
        public List<CombatLog> this[DataIndex combatIndex] => CombatLogTable[combatIndex];

        private Table<DataIndex, List<CombatLog>> CombatLogTable { get; } = new();


        public void Initialize(ActionTable<CombatLog> combatTakeAction)
        {
            combatTakeAction.Add("BattleLog", Add);
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
                    TotalDamaged += log.Value;
                    BeatCount++;
                    break;
                }
                case CombatEntityType.CriticalDamage:
                {
                    TotalDamaged += log.Value;
                    BeatCount++;
                    BeCriticalHitCount++;
                    break;
                }
                case CombatEntityType.Heal:
                {
                    TotalHealed += log.Value;
                    BeatCount++;
                    break;
                }
                case CombatEntityType.CriticalHeal:
                {
                    TotalHealed += log.Value;
                    BeatCount++;
                    BeCriticalHitCount++;
                    break;
                }
                // case CombatEntityType.Evade:          break;
                // case CombatEntityType.Block:          break;
                // case CombatEntityType.Absorb:         break;
                // case CombatEntityType.Immune:         break;
                default: return;
            }
        }
    }
}
