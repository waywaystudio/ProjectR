using System;
using Common.Characters;
using UnityEngine;

namespace Common.Runes
{
    [Serializable]
    public class RewardRune
    {
        public RewardRuneType RuneType { get; set; }
    }

    public class SkillRune
    {
        // ex LeapAttack
        public DataIndex TargetSkill;
        
        /// LeapAttack
        // JumpEnforceRune
        // StackEnforceRune
        // IncreaseDamageByJumpDistanceRune
        
        /// Deathblow
        // RapidProgressRune
        // IncreaseDamageByDistance


        public void Enforce(CharacterBehaviour cb)
        {
            
        }
    }
}
