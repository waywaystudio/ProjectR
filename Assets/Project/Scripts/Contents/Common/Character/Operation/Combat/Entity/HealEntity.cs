using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class HealEntity : BaseEntity, ICombatProvider
    {
        [ShowInInspector]
        private StatTable healTable = new();

        public string Name => Sender.Name;
        public GameObject Object => Sender.Object;
        public StatTable StatTable => healTable;

        public override bool IsReady => true;
        
        public void CombatReport(CombatLog log) => Sender.CombatReport(log);

        public override void SetEntity()
        {
            healTable.Register(StatCode.MultiPower, InstanceID, Data.BaseValue, true);
        }
        
        
        private void Start()
        {
            healTable.UnionWith(Sender.StatTable);
        }

        private void Reset()
        {
            flag = EntityType.Heal;
            SetEntity();
        }
    }
}
