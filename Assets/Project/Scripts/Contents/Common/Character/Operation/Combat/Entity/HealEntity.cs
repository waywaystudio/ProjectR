using Core;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class HealEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private float combatValue;

        public int ID => Skill.ID;
        public string ActionName => Skill.SkillName;
        public GameObject Object => Cb.gameObject;
        public string ProviderName => Cb.CharacterName;
        
        public float CombatPower => Cb.CombatPower * combatValue;
        public float Critical => Cb.Critical;
        public float Haste => Cb.Haste;
        public float Hit => 1.0f;

        public override bool IsReady => true;
        
        public void CombatReport(ILog log) => Cb.CombatReport(log);
        

        protected override void Awake()
        {
            base.Awake();

            SetEntity();
        }
        
        public override void SetEntity()
        {
            combatValue = SkillData.BaseValue;
        }

        private void Reset()
        {
            flag = EntityType.Heal;
            SetEntity();
        }
    }
}
