using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class HealEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private float combatValue;

        public int ID => AssignedSkill.ID;
        public string Name => Cb.CharacterName;
        public string ActionName => AssignedSkill.ActionName;
        public GameObject Object => Cb.gameObject;
        public ICombatProvider Predecessor => Cb;
        public CombatValueEntity CombatValue
        {
            get
            {
                var healValue = Cb.CombatValue;
                healValue.Power = Cb.CombatValue.Power * combatValue;
                healValue.Hit = 1.0f;

                return healValue;
            }
        }

        public override bool IsReady => true;
        
        public void CombatReport(CombatLog log) => Predecessor.CombatReport(log);
        

        protected override void Awake()
        {
            base.Awake();

            SetEntity();
        }
        
        public override void SetEntity()
        {
            // combatValue = SkillData.BaseValue;
        }

        private void Reset()
        {
            flag = EntityType.Heal;
            SetEntity();
        }
    }
}
