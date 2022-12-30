using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class StatusEffectEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private IDCode statusEffectCode;

        public new IDCode ActionCode { get => statusEffectCode; set => statusEffectCode = value; }
        public string Name => Sender.Name;
        public GameObject Object => Sender.Object;
        public Status Status => Sender.Status;
        public StatTable StatTable => Sender.StatTable;
        public void ReportActive(CombatLog log) => Sender.ReportActive(log);
        
        public override bool IsReady => true;
    }
}

/* Annotation
 StatusEffectEntity에 BaseValue를 바로 계산할 수 없다.
부패와 같이 데미지를 올려주어야 하는 baseValue가 있는 반면에
블러드 처럼 가속에 baseValue를 올려주는 기술이 있는 등, 상태이상에 성격에 따라서 다르기 때문이다.
그러나 Entity 내부에 ICombatProvider를 만들고, 인터페이스를 지울 필요는 또 없다.
이 스킬에 해당하는 기술의 특성이 올라갈 경우, 여기서 올려주는게 맞을 수도 있기 때문이다. */