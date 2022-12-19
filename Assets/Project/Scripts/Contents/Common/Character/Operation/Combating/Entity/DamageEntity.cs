using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class DamageEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private float combatValue;

        public int ID => Skill.ID;
        public string Name => Skill.SkillName;
        public float BaseCombatPower => Cb.CombatPower.Result;
        
        public GameObject Provider => Cb.gameObject;
        public string ProviderName => Cb.CharacterName;
        public float CombatPower => BaseCombatPower * combatValue;
        public float Critical => Cb.Critical.Result;
        public float Haste => Cb.Haste.Result;
        public float Hit => Cb.Hit.Result;

        public override bool IsReady => true;


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
