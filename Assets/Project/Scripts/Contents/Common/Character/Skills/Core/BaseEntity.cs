#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using System.Reflection;
#endif

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Skill = MainGame.Data.ContentData.SkillData.Skill;
using UnityEngine;

namespace Common.Character.Skills.Core
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField] protected BaseSkill skill;
        [SerializeField] protected EntityType flag;

        protected BaseSkill Skill => skill ??= GetComponent<BaseSkill>();
        protected Skill SkillData => Skill.SkillData;
        protected CharacterBehaviour Cb => Skill.Combat.Cb;

        public abstract bool IsReady { get; }

        public virtual void OnRegistered(){}
        public virtual void OnUnregistered(){}

        protected virtual void Awake()
        {
            if (!Skill.EntityTable.ContainsKey(flag))
                Skill.EntityTable.Add(flag, this);
        }

#if UNITY_EDITOR
        [OnInspectorInit]
        protected abstract void OnEditorInitialize();
#endif
    }
    
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
}
