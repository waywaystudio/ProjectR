#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif
using System;
using System.Collections.Generic;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;
using CombatClassData = MainGame.Data.ContentData.CombatClassData.CombatClass;

namespace Common.Character.Data
{
    public class ClassBaseStats : MonoBehaviour
    {
        // private const string BaseStatsCode = "BaseStats";

        [SerializeField] private string combatClass;
        [SerializeField] private float maxHp;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float critical;
        [SerializeField] private float haste;
        [SerializeField] private float hit;
        [SerializeField] private float evade;
        [SerializeField] private float armor;
        
        private CharacterBehaviour cb;
        private CombatClassData classData;
        private string characterName;
        private int instanceID;

        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private CombatClassData ClassData => classData ??= MainData.GetCombatClassData(combatClass);
        

        public void Initialize()
        {
            instanceID = GetInstanceID();
            combatClass.IsNullOrEmpty().OnTrue(() => combatClass = Cb.CombatClass);
            classData = MainData.GetCombatClassData(combatClass);

            maxHp = ClassData.MaxHp;
            moveSpeed = ClassData.MoveSpeed;
            critical = ClassData.Critical;
            haste = ClassData.Haste;
            hit = ClassData.Hit;
            evade = ClassData.Evade;
            armor = ClassData.Armor;
        }

        public void AddValueTable()
        {
            Cb.StatTable.Register(StatCode.AddMaxHp, instanceID, () => maxHp, true);
            Cb.StatTable.Register(StatCode.AddMoveSpeed, instanceID, () => moveSpeed, true);
            Cb.StatTable.Register(StatCode.AddCritical, instanceID, () => critical, true);
            Cb.StatTable.Register(StatCode.AddHaste, instanceID, () => haste, true);
            Cb.StatTable.Register(StatCode.AddHit, instanceID, () => hit, true);
            Cb.StatTable.Register(StatCode.AddEvade, instanceID, () => evade, true);
            Cb.StatTable.Register(StatCode.AddArmor, instanceID, () => armor, true);
        }
        
        private void Awake()
        {
            Cb.OnStart.Register(GetInstanceID(), Initialize);
            AddValueTable();
        }
    }
    
#if UNITY_EDITOR
    public class BaseStatsDrawer : OdinAttributeProcessor<ClassBaseStats>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Initialize")
            {
                attributes.Add(new PropertySpaceAttribute(15f, 0f));
                attributes.Add(new ButtonAttribute(ButtonSizes.Medium)
                {
                    Icon = SdfIconType.ArrowRepeat,
                    Stretch = false,
                });
            }
        }
    }
#endif
}
