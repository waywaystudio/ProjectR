using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class DamageEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private float damageValue;
        [ShowInInspector]
        private StatTable damageTable = new();

        public string Name => Sender.Name;
        public GameObject Object => Sender.Object;
        public Status Status => Sender.Status;
        public StatTable StatTable => damageTable;
        public void ReportActive(CombatLog log) => Sender.ReportActive(log);

        public override bool IsReady => true;
        public float DamageValue { get => damageValue; set => damageValue = value; }


        protected override void Awake()
        {
            base.Awake();
            damageTable.Register(StatCode.MultiPower, InstanceID, DamageValue, true);
        }

        private void Start()
        {
            damageTable.UnionWith(Sender.StatTable);
        }
    }
}

/*
 * Annotation
 * 모든 스킬은 어떠한 Entity를 통해서 반드시 ICombatProvider를 가진다.
 * 다만 인터페이스를 스킬 클래스에 직접구현하지 않는데, ICombatProvider가 여럿일 수 있기 때문이다.
 * 만약 기술 클래스에 직접적으로 인터페이스를 달면, 하나 뿐히 달지 못한다.
 */
