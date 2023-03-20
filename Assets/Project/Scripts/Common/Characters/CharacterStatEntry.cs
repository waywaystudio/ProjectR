using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterStatEntry : MonoBehaviour, IDynamicStatEntry, IEditable
    {
        [SerializeField] private DataIndex baseStatCode;
        [SerializeField] private MaxHpValue maxHp;
        [SerializeField] private MaxResourceValue maxResource;
        [SerializeField] private MoveSpeedValue moveSpeed;
        [SerializeField] private CriticalValue critical;
        [SerializeField] private HasteValue haste;
        [SerializeField] private ArmorValue armor;

        private CharacterBehaviour cb;
        
        public MaxHpValue MaxHp => maxHp;
        public MaxResourceValue MaxResource => maxResource;
        public MoveSpeedValue MoveSpeed => moveSpeed;
        public CriticalValue Critical => critical;
        public HasteValue Haste => haste;
        public ArmorValue Armor => armor;

        public AliveValue Alive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        public StatTable StatTable { get; } = new();
        [ShowInInspector]
        public StatusEffectTable DeBuffTable { get; } = new();
        public StatusEffectTable BuffTable { get; } = new();
        
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public void Initialize()
        {
            StatTable.Register(baseStatCode, maxHp, true);
            StatTable.Register(baseStatCode, moveSpeed, true);
            StatTable.Register(baseStatCode, maxResource, true);
            StatTable.Register(baseStatCode, critical, true);
            StatTable.Register(baseStatCode, haste, true);
            StatTable.Register(baseStatCode, armor, true);
            
            Hp.StatTable       = StatTable;
            Resource.StatTable = StatTable;
            Shield.StatTable   = StatTable;

            SetDynamicStatEntry();
        }


        public void SetDynamicStatEntry()
        {
            Alive.Value    = true;
            Hp.Value       = StatTable.MaxHp;
            Resource.Value = StatTable.MaxResource;
            Shield.Value   = 0;
        }
        

        private void OnEnable()
        {
            Cb.OnDeBuffTaken.Register("RegisterTable", DeBuffTable.Register);
            Cb.OnBuffTaken.Register("RegisterTable", BuffTable.Register);
        }

        private void OnDisable()
        {
            Cb.OnDeBuffTaken.Unregister("RegisterTable");
            Cb.OnBuffTaken.Unregister("RegisterTable");
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            baseStatCode = GetComponent<IDataIndexer>().ActionCode;
            
            maxHp.StatCode       = StatCode.MaxHp;
            moveSpeed.StatCode   = StatCode.MoveSpeed;
            maxResource.StatCode = StatCode.MaxResource;
            critical.StatCode    = StatCode.Critical;
            haste.StatCode       = StatCode.Haste;
            armor.StatCode       = StatCode.Armor;

            switch ((int)baseStatCode / 1000000)
            {
                case (int)DataIndex.CombatClass:
                {
                    var classData = Database.CombatClassSheetData(baseStatCode);
            
                    maxHp.Value       = classData.MaxHp;
                    moveSpeed.Value   = classData.MoveSpeed;
                    maxResource.Value = classData.MaxResource;
                    critical.Value    = classData.Critical;
                    haste.Value       = classData.Haste;
                    armor.Value       = classData.Armor;
                    break;
                }
                case (int)DataIndex.Boss:
                {
                    var bossData = Database.BossSheetData(baseStatCode);
            
                    maxHp.Value       = bossData.MaxHp;
                    moveSpeed.Value   = bossData.MoveSpeed;
                    maxResource.Value = bossData.MaxResource;
                    critical.Value    = bossData.Critical;
                    haste.Value       = bossData.Haste;
                    armor.Value       = bossData.Armor;
                    break;
                }
                default:
                {
                    Debug.LogWarning($"DataIndex Error. Must be CombatClass or Boss. Input:{(DataIndex)((int)baseStatCode / 100000)}");
                    return;
                } 
            }
        }
#endif
    }
}
