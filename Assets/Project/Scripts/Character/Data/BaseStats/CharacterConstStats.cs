using UnityEngine;

namespace Character.Data.BaseStats
{
    public class CharacterConstStats : MonoBehaviour, IEditorSetUp
    {
        [SerializeField] protected IDCode baseStatCode;
        [SerializeField] protected float maxHp;
        [SerializeField] protected float maxResource;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float critical;
        [SerializeField] protected float haste;
        [SerializeField] protected float hit;
        [SerializeField] protected float evade;
        [SerializeField] protected float armor;

        protected int InstanceID;
        protected CharacterBehaviour Cb { get; set; }


        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
            
            Cb = GetComponentInParent<CharacterBehaviour>();

            Cb.StatTable.Register(StatCode.AddMaxHp, InstanceID, () => maxHp, true);
            Cb.StatTable.Register(StatCode.AddMoveSpeed, InstanceID, () => moveSpeed, true);
            Cb.StatTable.Register(StatCode.AddMaxResource, InstanceID, () => maxResource, true);
            Cb.StatTable.Register(StatCode.AddCritical, InstanceID, () => critical, true);
            Cb.StatTable.Register(StatCode.AddHaste, InstanceID, () => haste, true);
            Cb.StatTable.Register(StatCode.AddHit, InstanceID, () => hit, true);
            Cb.StatTable.Register(StatCode.AddEvade, InstanceID, () => evade, true);
            Cb.StatTable.Register(StatCode.AddArmor, InstanceID, () => armor, true);
        }

#if UNITY_EDITOR
        public virtual void SetUp() {}
#endif
    }
}
