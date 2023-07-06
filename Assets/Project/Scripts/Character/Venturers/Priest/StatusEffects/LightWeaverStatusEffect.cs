using Character.Venturers.Priest.Skills;
using Common;
using Common.StatusEffects;

namespace Character.Venturers.Priest.StatusEffects
{
    public class LightWeaverStatusEffect : StatusEffect
    {
        private HealingTouch HealingTouch { get; set; }
        private LightOfDawn LightOfDawn { get; set; }
        
        private float healingTouchOriginalPower;
        private float lightOfDawnOriginalPower;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            HealingTouch = Provider.SkillBehaviour.GetSkill(DataIndex.HealingTouch) as HealingTouch;
            LightOfDawn  = Provider.SkillBehaviour.GetSkill(DataIndex.LightOfDawn) as LightOfDawn;

            Builder
                .Add(Section.Active, "EnforceHealingTouch", EnforceHealingTouch)
                .Add(Section.Active, "EnforceLightOfDawn", EnforceLightOfDawn)
                .Add(Section.End, "ReturnToOriginal", ReturnToOriginal);
        }


        private void EnforceHealingTouch()
        {
            if (HealingTouch is null) 
                return;

            var healSpec = HealingTouch.HealExecution.HealSpec;
            
            healingTouchOriginalPower = healSpec.Power;
            healSpec.Change(StatType.Power, healingTouchOriginalPower * 3f);
        }

        private void EnforceLightOfDawn()
        {
            if (LightOfDawn is null) 
                return;
            
            var healSpec = LightOfDawn.HealExecution.HealSpec;
            
            lightOfDawnOriginalPower = healSpec.Power;
            healSpec.Change(StatType.Power, lightOfDawnOriginalPower * 2f);
        }

        private void ReturnToOriginal()
        {
            var touchHealSpec = HealingTouch.HealExecution.HealSpec;
            var dawnHealSpec = LightOfDawn.HealExecution.HealSpec;
            
            touchHealSpec.Change(StatType.Power, healingTouchOriginalPower);
            dawnHealSpec.Change(StatType.Power, lightOfDawnOriginalPower);
        }
    }
}
