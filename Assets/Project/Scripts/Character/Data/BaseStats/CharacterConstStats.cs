using Core;
using UnityEngine;

namespace Character.Data.BaseStats
{
    public class CharacterConstStats : MonoBehaviour, IInspectorSetUp
    {
        [SerializeField] protected DataIndex baseStatCode;
        [SerializeField] protected MaxHpValue maxHp;
        [SerializeField] protected MaxResourceValue maxResource;
        [SerializeField] protected MoveSpeedValue moveSpeed;
        [SerializeField] protected CriticalValue critical;
        [SerializeField] protected HasteValue haste;
        [SerializeField] protected HitValue hit;
        [SerializeField] protected EvadeValue evade;
        [SerializeField] protected ArmorValue armor;
        

        public void Initialize(StatTable statTable)
        {
            statTable.Register(baseStatCode, maxHp, true);
            statTable.Register(baseStatCode, moveSpeed, true);
            statTable.Register(baseStatCode, maxResource, true);
            statTable.Register(baseStatCode, critical, true);
            statTable.Register(baseStatCode, haste, true);
            statTable.Register(baseStatCode, hit, true);
            statTable.Register(baseStatCode, evade, true);
            statTable.Register(baseStatCode, armor, true);
        }

#if UNITY_EDITOR
        public virtual void SetUp()
        {
            maxHp.StatCode       = StatCode.MaxHp;
            moveSpeed.StatCode   = StatCode.MoveSpeed;
            maxResource.StatCode = StatCode.MaxResource;
            critical.StatCode    = StatCode.Critical;
            haste.StatCode       = StatCode.Haste;
            hit.StatCode         = StatCode.Hit;
            evade.StatCode       = StatCode.Evade;
            armor.StatCode       = StatCode.Armor;
        }
#endif
    }
}
