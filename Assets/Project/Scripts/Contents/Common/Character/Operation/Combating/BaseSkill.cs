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
        protected int InstanceID;
        
        private Combat combat;
        private CharacterBehaviour cb;
        private Skill skillData;

        public int ID => id;
        public Combat Combat => combat ??= GetComponentInParent<Combat>();
        public CharacterBehaviour Cb => cb ??= Combat.Cb;
        public string SkillName => skillName ??= GetType().Name;
        public Skill SkillData => skillData ??= MainData.GetSkillData(SkillName);
        
        public string AnimationKey { get; private set; }
        public int Priority { get; set; }
        public bool IsSkillFinished { get; set; }
        public Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();
        
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady =>!EntityTable.ContainsKey(EntityType.CoolTime) || 
                                       EntityTable[EntityType.CoolTime].IsReady;

        public ActionTable OnStarted { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public ActionTable OnCompleted { get; } = new();

        public void StartSkill()
        {
            OnStarted?.Invoke();
            IsSkillFinished = false;
        }
        
        public void InterruptedSkill() => OnInterrupted?.Invoke();

        public virtual void CompleteSkill()
        {
            OnCompleted?.Invoke();
            IsSkillFinished = true;
        }
        
        public virtual void InvokeEvent(){}

        public void ActiveSkill()
        {
            switch (AnimationKey)
            {
                case "Attack":
                {
                    Cb.OnAttack.Register(InstanceID, StartSkill);
                    Cb.OnAttackHit.Register(InstanceID, InvokeEvent);
                    Cb.Attack(SkillName, CompleteSkill);
                    break;
                }
                case "Skill":
                {
                    Cb.OnSkill.Register(InstanceID, StartSkill);
                    Cb.OnSkillHit.Register(InstanceID, InvokeEvent);
                    Cb.Skill(SkillName, CompleteSkill);
                    break;
                }
                case "Channeling":
                {
                    Cb.OnChanneling.Register(InstanceID, StartSkill);
                    Cb.OnChannelingHit.Register(InstanceID, InvokeEvent);
                    Cb.Channeling(SkillName, CompleteSkill);
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
                    Cb.OnAttack.UnRegister(InstanceID);
                    Cb.OnAttackHit.UnRegister(InstanceID);
                    break;
                }
                case "Skill":
                {
                    Cb.OnSkill.UnRegister(InstanceID);
                    Cb.OnSkillHit.UnRegister(InstanceID);
                    break;
                }
                case "Channeling":
                {
                    Cb.OnChanneling.UnRegister(InstanceID);
                    Cb.OnChannelingHit.UnRegister(InstanceID);
                    break;
                }
                // add more case according to Animation, DamageMechanic...
            }
        }

        public bool TryGetEntity<T>(EntityType entityType, out T result) where T : BaseEntity
        {
            var hasEntity = EntityTable.TryGetValue(entityType, out var entity);

            if (hasEntity)
            {
                result = entity as T;
                return true;
            }

            result = null;
            return false;
        }


        protected void Awake()
        {
            skillName ??= GetType().Name;
            AnimationKey = SkillData.AnimationKey;
            Priority = SkillData.Priority;
            InstanceID = GetInstanceID();
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
        // [OnInspectorInit]
        // protected virtual void InEditorOnInit() => InEditorGetData();

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
                // attributes.Add(new DisplayAsStringAttribute());
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
