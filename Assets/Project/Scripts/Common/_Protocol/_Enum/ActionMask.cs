using System;

namespace Common
{
    [Flags]
    public enum ActionMask
    {
        None = 0,
        Stop = 1      << 0,
        Run = 1       << 1,
        Dead = 1      << 2,
        KnockBack = 1 << 3,
        Stun = 1      << 4,
        Skill = 1     << 5, 
        RigidSkill = 1 << 6,
        Draw = 1      << 7,

        All = int.MaxValue
    }

    public static class ActionMaskExtension
    {
        /*
         * Action Mask Ignore Matrix
         */
        public static bool CanOverride(this ActionMask mask, ActionMask target)
        {
            const ActionMask runIgnoreMask = ActionMask.None | 
                                             ActionMask.Stop | 
                                             ActionMask.Run  | 
                                             ActionMask.Skill;
            
            const ActionMask skillIgnoreMask = ActionMask.None | 
                                               ActionMask.Stop | 
                                               ActionMask.Run;
            
            const ActionMask knockBackIgnoreMask = ActionMask.None | 
                                                   ActionMask.Stop | 
                                                   ActionMask.Run  | 
                                                   ActionMask.Skill;
            
            const ActionMask drawIgnoreMask = ActionMask.None | 
                                              ActionMask.Stop | 
                                              ActionMask.Run  | 
                                              ActionMask.Skill;
            
            const ActionMask stunIgnoreMask = ActionMask.None | 
                                              ActionMask.Stop | 
                                              ActionMask.Run  | 
                                              ActionMask.Skill;
            
            const ActionMask deadIgnoreMask = ActionMask.None      | 
                                              ActionMask.Stop      | 
                                              ActionMask.Run       | 
                                              ActionMask.Stun      | 
                                              ActionMask.KnockBack | 
                                              ActionMask.Skill     | 
                                              ActionMask.Draw      |
                                              ActionMask.RigidSkill;

            return mask switch
            {
                ActionMask.None       => false,
                ActionMask.Stop       => (mask                | target) == mask,
                ActionMask.Run        => (runIgnoreMask       | target) == runIgnoreMask,
                ActionMask.Dead       => (deadIgnoreMask      | target) == deadIgnoreMask,
                ActionMask.KnockBack  => (knockBackIgnoreMask | target) == knockBackIgnoreMask,
                ActionMask.Draw       => (drawIgnoreMask      | target) == drawIgnoreMask,
                ActionMask.Stun       => (stunIgnoreMask      | target) == stunIgnoreMask,
                ActionMask.Skill      => (skillIgnoreMask     | target) == skillIgnoreMask,
                ActionMask.RigidSkill => (skillIgnoreMask     | target) == skillIgnoreMask,
                _                     => false
            };
        } 
            
    }
}