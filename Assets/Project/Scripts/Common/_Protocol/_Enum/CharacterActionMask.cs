using System;

namespace Common
{
    [Flags]
    public enum CharacterActionMask
    {
        None = 0,
        Stop = 1       << 0,
        Run = 1        << 1,
        Dead = 1       << 2,
        KnockBack = 1  << 3,
        Stun = 1       << 4,
        Skill = 1      << 5,
        RigidSkill = 1 << 6,
        
        /* Preset */
        RunIgnoreMask = None       | Stop | Run | Skill,
        SkillIgnoreMask = None     | Stop | Run,
        KnockBackIgnoreMask = None | Stop | Run  | Skill,
        StunIgnoreMask = None      | Stop | Run  | Skill,
        DeadIgnoreMask = None      | Run  | Stop | Stun | KnockBack | Skill | RigidSkill, 
        
        All = int.MaxValue
    }
}