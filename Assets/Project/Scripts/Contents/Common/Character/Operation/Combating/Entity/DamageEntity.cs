using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class DamageEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private float combatValue;

        public int ID => Skill.ID;
        public string Name => Skill.SkillName;
        public GameObject Provider => Cb.gameObject;
        public string ProviderName => Cb.CharacterName;
        public float CombatPower => Cb.CombatPowerTable.Result * combatValue;
        public float Critical => Cb.CriticalTable.Result;
        public float Haste => Cb.HasteTable.Result;
        public float Hit => Cb.HitTable.Result;

        public override bool IsReady => true;
        
        public void CombatReport(ILog log)
        {
            if (log is CombatLog combatLog)
            {
                Cb.ReportDamage(combatLog);
            } 
        }

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
            flag = EntityType.Damage;
            SetEntity();
        }
    }
}
