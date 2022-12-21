using Core;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class DamageEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private float combatValue;

        private CombatEntity skillCombatEntity;
        public CombatEntity CombatEntity => skillCombatEntity + Cb.CombatEntity;

        public int ID => Skill.ID;
        public string ActionName => Skill.SkillName;
        public string ProviderName => Cb.CharacterName;
        public GameObject Object => Cb.gameObject;
        
        
        public float CombatPower => Cb.CombatPower * combatValue;
        public float Critical => Cb.Critical;
        public float Haste => Cb.Haste;
        public float Hit => Cb.Hit;

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
            skillCombatEntity = new CombatEntity
                           {
                                   CombatPower = SkillData.BaseValue
                                   // Critical = 
                                   // Haste = 
                                   // Hit = 
                           };
        }

        private void Reset()
        {
            flag = EntityType.Damage;
            SetEntity();
        }
    }
}
