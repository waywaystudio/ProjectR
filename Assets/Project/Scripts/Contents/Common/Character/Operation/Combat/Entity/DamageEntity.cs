using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class DamageEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private float combatValue;

        public int ID => AssignedSkill.ID;
        public string ActionName => AssignedSkill.ActionName;
        public string Name => Cb.Name;
        public GameObject Object => Cb.Object;
        public ICombatProvider Predecessor => Cb;
        
        public CombatValueEntity CombatValue
        {
            get
            {
                var healValue = Cb.CombatValue;
                healValue.Power = Cb.CombatValue.Power * combatValue;

                return healValue;
            }
        }

        public override bool IsReady => true;
        
        public void CombatReport(CombatLog log) => Cb.CombatReport(log);

        protected override void Awake()
        {
            base.Awake();
            SetEntity();
        }

        public override void SetEntity()
        {
            var skillData = MainGame.MainData.GetSkillData(AssignedSkill.ActionName);
            combatValue = skillData.BaseValue;
        }

        private void Reset()
        {
            flag = EntityType.Damage;
            SetEntity();
        }
    }
}

/*
 * Annotation
 * 모든 스킬은 어떠한 Entity를 통해서 반드시 ICombatProvider를 가진다.
 * 다만 인터페이스를 스킬 클래스에 직접구현하지 않는데, ICombatProvider가 여럿일 수 있기 때문이다.
 * 만약 기술 클래스에 직접적으로 인터페이스를 달면, 하나 뿐히 달지 못한다.
 */
