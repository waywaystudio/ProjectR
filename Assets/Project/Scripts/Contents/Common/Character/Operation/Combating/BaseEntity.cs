#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Skill = MainGame.Data.ContentData.SkillData.Skill;

namespace Common.Character.Operation.Combating
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField] protected EntityType flag;
        [SerializeField] private BaseSkill skill;
        protected int InstanceID;
        
        protected BaseSkill Skill => skill ??= GetComponent<BaseSkill>();
        protected Skill SkillData => Skill.SkillData;
        protected CharacterBehaviour Cb => Skill.Combat.Cb;

        [ShowInInspector]
        public abstract bool IsReady { get; }

        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
            
            skill ??= GetComponent<BaseSkill>();
            Skill.EntityTable.TryAdd(flag, this);
        }
    }

#if UNITY_EDITOR
    public class BaseEntityDrawer : OdinAttributeProcessor<BaseEntity>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "skill")
            {
                attributes.Add(new HideInInspector());
            }
        }
    }
#endif
}
