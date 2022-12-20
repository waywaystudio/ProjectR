using Core;

namespace Common.Character
{
    public class CombatLog : ILog
    {
        public string Provider;
        public string Taker;
        public string SkillName;
        public bool IsHit;
        public bool IsCritical;
        public float Value;
    }
}
