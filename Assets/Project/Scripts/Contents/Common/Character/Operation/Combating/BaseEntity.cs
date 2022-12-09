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
            Skill.EntityTable.TryAdd(flag, this);
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
