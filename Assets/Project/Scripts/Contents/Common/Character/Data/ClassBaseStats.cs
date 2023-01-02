#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using CombatClassData = MainGame.Data.ContentData.CombatClassData.CombatClass;

namespace Common.Character.Data
{
    public class ClassBaseStats : MonoBehaviour
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
        protected virtual void SetUp() {}
#endif
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
