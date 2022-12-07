namespace Common.Character.Skills.Entity
{
    public class DamageEntity : EntityAttribution
    {
        public double CombatValue { get; set; }
        public float AdditionalValue { get; set; }
        public float CriticalChance { get; set; }
        public float HitChance { get; set; }
        public float AdditionalAggro { get; set; }

        public void SetEntity()
        {
            // CombatValue = Cb.BaseStats.CombatValue
            AdditionalValue = SkillData.BaseValue;
            // CriticalChance = Cb.BaseStats.CriticalChance
            // HitChance = Cb.BaseStats.HitChance
            // AdditionalAggro = Cb.BaseStats.AdditionalAggro
        }

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            Flag = EntityType.Damage;

            SetEntity();
        }
#endif
    }
}
