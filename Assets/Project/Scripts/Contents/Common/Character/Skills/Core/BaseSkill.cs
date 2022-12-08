#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif

using System;
using MainGame;
using Skill = MainGame.Data.ContentData.SkillData.Skill;
using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

/*
 * Buff
 * Casting
 * CoolTime
 * Damage
 * Heal
 * Position
 * Projectile
 * Target
 */

namespace Common.Character.Skills.Core
{
    public class BaseSkill : MonoBehaviour
    {
        [SerializeField] protected int id;
        [SerializeField] protected string skillName;

        private Combat combat;
        private Skill skillData;

        public Combat Combat => combat ??= GetComponentInParent<Combat>();
        public string SkillName => skillName ??= GetType().Name;
        public Skill SkillData => skillData ??= MainData.GetSkillData(skillName);
        public Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();

        public string AnimationKey { get; private set; }
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady =>!EntityTable.ContainsKey(EntityType.CoolTime) || 
                                       EntityTable[EntityType.CoolTime].IsReady;

        [ShowInInspector] public Action OnStarted { get; set; }
        [ShowInInspector] public Action OnInterrupted { get; set; }
        [ShowInInspector] public Action OnCompleted { get; set; }

        
        public virtual void StartSkill() => OnStarted?.Invoke();
        public virtual void InterruptedSkill() => OnInterrupted?.Invoke();
        public virtual void CompleteSkill() => OnCompleted?.Invoke();
        public virtual void InvokeEvent(){}
        
        public void Register() => EntityTable.ForEach(x => x.Value.OnRegistered());
        public void UnRegister() => EntityTable.ForEach(x => x.Value.OnUnregistered());

        protected void Awake()
        {
            Combat.SkillTable.TryAdd(id, this);
        }

#if UNITY_EDITOR
        #region EditorOnly
        [OnInspectorInit]
        protected virtual void InEditorOnInit() => InEditorGetData();

        [Button]
        protected void InEditorGetData()
        {
            var skillDb = MainData.GetSkillData(SkillName);
            
            id = skillDb.ID;
            AnimationKey = skillDb.AnimationKey;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endregion
#endif
    }

#if UNITY_EDITOR
    public class BaseSkillDrawer : OdinAttributeProcessor<BaseSkill>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "id")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "skillName")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
            
            if (member.Name == "SkillData")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "OnStarted")
            {
                attributes.Add(new ShowIfAttribute("@UnityEngine.Application.isPlaying"));
            }
            
            if (member.Name == "OnInterrupted")
            {
                attributes.Add(new ShowIfAttribute("@UnityEngine.Application.isPlaying"));
            }
            
            if (member.Name == "OnCompleted")
            {
                attributes.Add(new ShowIfAttribute("@UnityEngine.Application.isPlaying"));
            }
        }
    }
#endif
}
