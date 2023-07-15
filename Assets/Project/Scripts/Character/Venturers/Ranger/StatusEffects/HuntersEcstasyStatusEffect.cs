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

            FocusStrike = provider.SkillTable[DataIndex.FocusedStrike];

            if (!Verify.IsNotNull(FocusStrike, $"Not Exist FocusStrike in Skill List")) return;

            OriginalCastTime = FocusStrike.CastTimer.OriginalCastDuration;
            Builder
                .Add(Section.Active, "EnforceFocusStrike", EnforceFocusStrike)
                .Add(Section.End, "ResetFocusStrike", ResetFocusStrike);
        }


        // Add Active
        private void EnforceFocusStrike()
        {
            FocusStrike.CastTimer.OriginalCastDuration = 0f;
        }
        
        // End
        private void ResetFocusStrike()
        {
            FocusStrike.CastTimer.OriginalCastDuration = OriginalCastTime;
        }
        
    }
}
