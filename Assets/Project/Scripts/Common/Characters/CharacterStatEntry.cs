using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterStatEntry : MonoBehaviour, IDynamicStatEntry, IEditable
    {
        [SerializeField] private DataIndex baseStatCode;
        [SerializeField] private Spec combatClassSpec = new();
        // [SerializeField] private Equipments equipments;
        
        [SerializeField] private CriticalValue critical;
        [SerializeField] private MaxHpValue maxHp;
        [SerializeField] private MaxResourceValue maxResource;
        [SerializeField] private MoveSpeedValue moveSpeed;
        [SerializeField] private HasteValue haste;
        [SerializeField] private ArmorValue armor;

        public AliveValue Alive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        
        public StatTable StatTable { get; } = new();
        [ShowInInspector] public OldStatTable OldStatTable { get; } = new();
        [ShowInInspector] public StatusEffectTable DeBuffTable { get; } = new();
        [ShowInInspector] public StatusEffectTable BuffTable { get; } = new();
        

        public void Initialize()
        {
            // combatClassSpec.Register(StatTable);
            
            OldStatTable.Register(baseStatCode, maxHp, true);
            OldStatTable.Register(baseStatCode, moveSpeed, true);
            OldStatTable.Register(baseStatCode, maxResource, true);
            OldStatTable.Register(baseStatCode, critical, true);
            OldStatTable.Register(baseStatCode, haste, true);
            OldStatTable.Register(baseStatCode, armor, true);
            
            Hp.StatTable       = OldStatTable;
            Resource.StatTable = OldStatTable;
            Shield.StatTable   = OldStatTable;

            SetDynamicStatEntry();
        }

        public void SetDynamicStatEntry()
        {
            Alive.Value    = true;
            Hp.Value       = OldStatTable.MaxHp;
            Resource.Value = OldStatTable.MaxResource;
            Shield.Value   = 0;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            baseStatCode = GetComponent<IDataIndexer>().ActionCode;
            
            maxHp.StatCode       = StatType.MaxHp;
            moveSpeed.StatCode   = StatType.MoveSpeed;
            maxResource.StatCode = StatType.MaxResource;
            critical.StatCode    = StatType.CriticalChance;
            haste.StatCode       = StatType.Haste;
            armor.StatCode       = StatType.Armor;
            
            combatClassSpec.Clear();

            switch ((int)baseStatCode / 1000000)
            {
                case (int)DataIndex.CombatClass:
                {
                    var classData = Database.CombatClassSheetData(baseStatCode);

                    // combatClassSpec.Add(StatCode.Critical,    StatApplyType.Plus, classData.Critical);
                    // combatClassSpec.Add(StatCode.Haste,       StatApplyType.Plus, classData.Haste);
                    // combatClassSpec.Add(StatCode.Armor,       StatApplyType.Plus, classData.Armor);
                    // combatClassSpec.Add(StatCode.MaxHp,       StatApplyType.Plus, classData.MaxHp);
                    // combatClassSpec.Add(StatCode.MaxResource, StatApplyType.Plus, classData.MaxResource);
                    // combatClassSpec.Add(StatCode.MoveSpeed,   StatApplyType.Plus, classData.MoveSpeed);
            
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
                    
                    // combatClassSpec.Add(StatCode.Critical,    StatApplyType.Plus, bossData.Critical);
                    // combatClassSpec.Add(StatCode.Haste,       StatApplyType.Plus, bossData.Haste);
                    // combatClassSpec.Add(StatCode.Armor,       StatApplyType.Plus, bossData.Armor);
                    // combatClassSpec.Add(StatCode.MaxHp,       StatApplyType.Plus, bossData.MaxHp);
                    // combatClassSpec.Add(StatCode.MaxResource, StatApplyType.Plus, bossData.MaxResource);
                    // combatClassSpec.Add(StatCode.MoveSpeed,   StatApplyType.Plus, bossData.MoveSpeed);
            
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
