namespace Common.Character
{
    public class CombatLog
    {
        public CombatLog(string provider = "", string taker = "", string actionName = "", bool isHit = false, bool isCritical = false, float value = 0f)
        {
            Provider = provider;
            Taker = taker;
            ActionName = actionName;
            IsHit = isHit;
            IsCritical = isCritical;
            Value = value;
        }
        
        public readonly string Provider;
        public readonly string Taker;
        public readonly string ActionName;
        public bool IsHit;
        public bool IsCritical;
        public float Value;
        // public bool IsFinishHit
    }
}
