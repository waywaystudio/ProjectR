using UnityEngine;

namespace Core
{
    public interface ICombatProvider
    {
        int ID { get; }
        string ActionName { get; }
        string ProviderName { get; }
        GameObject Object { get; }
        
        float CombatPower { get; }
        float Critical { get; }
        float Haste { get; }
        float Hit { get; }
        // CombatEntity CombatEntity { get; }
        // ICombatProvider Origin { get; }

        void CombatReport(ILog log);
    }

    [System.Serializable]
    public struct CombatEntity
    {
        public CombatEntity(CombatEntity reference) 
                : this(reference.CombatPower, reference.Critical, reference.Haste,reference.Hit) {}
        public CombatEntity(float combatPower, float critical, float haste, float hit)
        {
            CombatPower = combatPower;
            Critical = critical;
            Haste = haste;
            Hit = hit;
        }
        
        public float CombatPower { get; set; }
        public float Critical { get; set; }
        public float Haste { get; set; }
        public float Hit { get; set; }

        public static CombatEntity operator +(CombatEntity entityLeft, CombatEntity entityRight)
        {
            return new CombatEntity
                    (
                     entityLeft.CombatPower + entityRight.CombatPower,
                     entityLeft.Critical + entityRight.Critical,
                     entityLeft.Haste + entityRight.Haste,
                     entityLeft.Hit + entityRight.Hit
                    );
        }
    }
}