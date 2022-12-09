#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using System.Reflection;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;
using Skill = MainGame.Data.ContentData.SkillData.Skill;

/* EntityList
 * Buff
 * Casting
 * CoolTime
 * Damage
 * Heal
 * Position
 * Projectile
 * Target
 */

namespace Common.Character.Operation.Combating
{
    public class BaseSkill : MonoBehaviour
    {
        [SerializeField] protected int id;
        [SerializeField] protected string skillName;
        
        private Combat combat;
        private CharacterBehaviour cb;
        private Skill skillData;

        public Combat Combat => combat ??= GetComponentInParent<Combat>();
        public CharacterBehaviour Cb => cb ??= Combat.Cb;
        public string SkillName => skillName ??= GetType().Name;
        public Skill SkillData => skillData ??= MainData.GetSkillData(skillName);
        
        public string AnimationKey { get; private set; }
        public int Priority { get; set; }
        [ShowInInspector]
        public bool IsSkillFinished { get; set; }
        public Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();
        
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady =>!EntityTable.ContainsKey(EntityType.CoolTime) || 
                                       EntityTable[EntityType.CoolTime].IsReady;

        public Action OnStarted { get; set; }
        public Action OnInterrupted { get; set; }
        public Action OnCompleted { get; set; }

        public virtual void StartSkill()
        {
            OnStarted?.Invoke();
            IsSkillFinished = false;
        }
        
        public virtual void InterruptedSkill() => OnInterrupted?.Invoke();

        public virtual void CompleteSkill()
        {
            OnCompleted?.Invoke();
            IsSkillFinished = true;
        }
        
        public virtual void InvokeEvent(){}

        public void OnActiveSkill()
        {
            switch (AnimationKey)
            {
                case "Attack":
                {
                    Cb.OnAttack += StartSkill;
                    Cb.OnAttackHit += InvokeEvent;
                    Cb.Attack();
                    break;
                }
                case "Skill":
                {
                    Cb.OnSkill += StartSkill;
                    Cb.OnSkillHit += InvokeEvent;
                    Cb.Skill();
                    break;
                }
                // add more case according to Animation, DamageMechanic...
            }
        }

        public void DeActiveSkill()
        {
            switch (AnimationKey)
            {
                case "Attack":
                {
                    Cb.OnAttack -= StartSkill;
                    Cb.OnAttackHit -= InvokeEvent;
                    break;
                }
                case "Skill":
                {
                    Cb.OnSkill -= StartSkill;
                    Cb.OnSkillHit -= InvokeEvent;
                    break;
                }
                // add more case according to Animation, DamageMechanic...
            }
        }


        protected void Awake()
        {
            skillName ??= GetType().Name;
            AnimationKey = SkillData.AnimationKey;
            Priority = SkillData.Priority;
        }

        protected void OnEnable()
        {
            Combat.SkillTable.TryAdd(id, this);
        }

        protected void OnDisable()
        {
            Combat.SkillTable.RemoveSafely(id);
            DeActiveSkill();
        }


#if UNITY_EDITOR
        #region EditorOnly
        [OnInspectorInit]
        protected virtual void InEditorOnInit() => InEditorGetData();

        [Button]
        protected void InEditorGetData()
        {
            id = SkillData.ID;
            AnimationKey = SkillData.AnimationKey;
            Priority = SkillData.Priority;
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
