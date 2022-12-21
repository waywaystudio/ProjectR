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
        private const string BaseStatsKey = "BaseStats";
        
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

        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private CombatClassData ClassData => classData ??= MainData.GetCombatClassData(combatClass);
        

        public void Initialize()
        {
            combatClass.IsNullOrEmpty().OnTrue(() => combatClass = Cb.CombatClass);
            classData = MainData.GetCombatClassData(combatClass);

            maxHp = ClassData.MaxHp;
            moveSpeed = ClassData.MoveSpeed;
            critical = ClassData.Critical;
            haste = ClassData.Haste;
            hit = ClassData.Hit;
            evade = ClassData.Evade;
            armor = ClassData.Armor;
            
            AddValueTable();
        }

        public void AddValueTable()
        {
            Cb.MaxHpTable.Register(BaseStatsKey, maxHp);
            Cb.MoveSpeedTable.Register(BaseStatsKey, moveSpeed);
            Cb.CriticalTable.Register(BaseStatsKey, critical);
            Cb.HasteTable.Register(BaseStatsKey, haste);
            Cb.HitTable.Register(BaseStatsKey, hit);
            Cb.EvadeTable.Register(BaseStatsKey, evade);
            Cb.ArmorTable.Register(BaseStatsKey, armor);
        }
        
        private void Awake()
        {
            Cb.OnStart.Register(GetInstanceID(), Initialize);
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
