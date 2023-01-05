using UnityEngine;

namespace Character.Combat.Entities
{
    public class HealEntity : BaseEntity, ICombatEntity
    {
        [SerializeField] private float healValue;

        public Status Status => Provider.Status;
        public StatTable StatTable { get; } = new();

        public override bool IsReady => true;
        public float HealValue { get => healValue; set => healValue = value; }

        private void Awake()
        {
            StatTable.Register(StatCode.MultiPower, InstanceID, HealValue, true);
        }

        private void Start()
        {
            StatTable.UnionWith(Provider.StatTable);
        }
    }
}
