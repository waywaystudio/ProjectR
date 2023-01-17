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

        protected CharacterBehaviour Cb { get; set; }


        protected virtual void Awake()
        {
            Cb = GetComponentInParent<CharacterBehaviour>();

            Cb.StatTable.Register(baseStatCode, maxHp, true);
            Cb.StatTable.Register(baseStatCode, moveSpeed, true);
            Cb.StatTable.Register(baseStatCode, maxResource, true);
            Cb.StatTable.Register(baseStatCode, critical, true);
            Cb.StatTable.Register(baseStatCode, haste, true);
            Cb.StatTable.Register(baseStatCode, hit, true);
            Cb.StatTable.Register(baseStatCode, evade, true);
            Cb.StatTable.Register(baseStatCode, armor, true);
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
