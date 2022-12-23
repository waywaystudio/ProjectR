using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class HealEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private float healValue;
        [ShowInInspector]
        private StatTable healTable = new();

        public string Name => Sender.Name;
        public GameObject Object => Sender.Object;
        public StatTable StatTable => healTable;

        public override bool IsReady => true;
        public float HealValue { get => healValue; set => healValue = value; }

        public void CombatReport(CombatLog log) => Sender.CombatReport(log);
        
        

        protected override void Awake()
        {
            base.Awake();
            healTable.Register(StatCode.MultiPower, InstanceID, HealValue, true);
        }

        private void Start()
        {
            healTable.UnionWith(Sender.StatTable);
        }

        private void Reset()
        {
            flag = EntityType.Heal;
        }
    }
}
