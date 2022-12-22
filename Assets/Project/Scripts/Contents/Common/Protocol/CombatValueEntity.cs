namespace Common
{
    [System.Serializable]
    public struct CombatValueEntity
    {
        public CombatValueEntity(CombatValueEntity reference) 
            : this(reference.Power, reference.Critical, reference.Haste,reference.Hit) {}
        public CombatValueEntity(float power, float critical, float haste, float hit)
        {
            Power = power;
            Critical = critical;
            Haste = haste;
            Hit = hit;
        }
        
        public float Power { get; set; }
        public float Critical { get; set; }
        public float Haste { get; set; }
        public float Hit { get; set; }

        public static CombatValueEntity operator +(CombatValueEntity valueEntityLeft, CombatValueEntity valueEntityRight)
        {
            return new CombatValueEntity
            (
                valueEntityLeft.Power + valueEntityRight.Power,
                valueEntityLeft.Critical + valueEntityRight.Critical,
                valueEntityLeft.Haste + valueEntityRight.Haste,
                valueEntityLeft.Hit + valueEntityRight.Hit
            );
        }
    }
}
