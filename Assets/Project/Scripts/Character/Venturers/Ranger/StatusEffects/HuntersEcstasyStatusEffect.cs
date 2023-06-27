using Common;
using Common.Skills;
using Common.StatusEffects;

namespace Character.Venturers.Ranger.StatusEffects
{
    public class HuntersEcstasyStatusEffect : StatusEffect
    {
        private SkillComponent FocusStrike { get; set; }
        private float OriginalCastTime { get; set; }

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            FocusStrike = provider.SkillBehaviour.GetSkill(DataIndex.FocusedStrike);

            if (!Verify.IsNotNull(FocusStrike, $"Not Exist FocusStrike in Skill List")) return;

            OriginalCastTime = FocusStrike.CastTimer.OriginalCastingTime;
            SequenceBuilder
                .Add(SectionType.Active, "EnforceFocusStrike", EnforceFocusStrike)
                .Add(SectionType.End, "ResetFocusStrike", ResetFocusStrike);
        }


        // Add Active
        private void EnforceFocusStrike()
        {
            FocusStrike.CastTimer.CastingTime = 0f;
        }
        
        // End
        private void ResetFocusStrike()
        {
            FocusStrike.CastTimer.CastingTime = OriginalCastTime;
        }
        
    }
}
