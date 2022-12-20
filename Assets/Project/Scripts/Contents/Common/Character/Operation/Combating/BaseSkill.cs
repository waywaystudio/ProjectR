#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using System.Reflection;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using MainGame;
using MainGame.Data;
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
        [SerializeField] protected string animationKey;
        [SerializeField] protected int priority;
        
        protected int InstanceID;
        
        private Combat combat;
        private CharacterBehaviour cb;
        private Skill skillData;

        public int ID => id;
        public string SkillName => skillName ??= GetType().Name;
        public int Priority { get => priority; set => priority = value; }
        
        public Combat Combat => combat ??= GetComponentInParent<Combat>();
        public CharacterBehaviour Cb => cb ??= Combat.Cb;
        public Skill SkillData => skillData ??= MainData.GetSkillData(SkillName);
        public Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();
        
        public bool IsSkillFinished { get; set; }
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady =>!EntityTable.ContainsKey(EntityType.CoolTime) || 
                                       EntityTable[EntityType.CoolTime].IsReady;

        public ActionTable OnStarted { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public ActionTable OnCompleted { get; } = new();

        public virtual void StartSkill()
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
        
        /// <summary>
        /// Register Animation Event.
        /// </summary>
        public virtual void InvokeEvent(){}

        public virtual void ActiveSkill()
        {
            Cb.OnSkill.Register(InstanceID, StartSkill);
            Cb.OnSkillHit.Register(InstanceID, InvokeEvent);
            Cb.Skill(SkillName, CompleteSkill);
        }

        public void DeActiveSkill()
        {
            Cb.OnSkill.UnRegister(InstanceID);
            Cb.OnSkillHit.UnRegister(InstanceID);
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
            skillData = MainData.GetSkillData(skillName);
            animationKey = SkillData.AnimationKey;
            priority = SkillData.Priority;
            InstanceID = GetInstanceID();
        }

        protected void OnEnable()
        {
            Combat.SkillTable.TryAdd(id, this);
        }

        protected void OnDisable()
        {
            Combat.SkillTable.TryRemove(id);
            DeActiveSkill();
        }


#if UNITY_EDITOR
        #region EditorOnly
        protected void GetDataFromDB()
        {
            skillName = GetType().Name;
            skillData = MainData.GetSkillData(skillName);
            id = skillData.ID;
            animationKey = skillData.AnimationKey;
            Priority = skillData.Priority;
            UnityEditor.EditorUtility.SetDirty(this);

            GetComponents<BaseEntity>().ForEach(x => x.SetEntity());
        }
        
        protected void ShowDB()
        {
            UnityEditor.EditorUtility.OpenPropertyEditor(MainData.DataObjectList.Find(x => x.Category == DataCategory.Skill));
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
            
            if (member.Name == "GetDataFromDB")
            {
                attributes.Add(new HorizontalGroupAttribute("Editor Functions"));
                attributes.Add(new PropertySpaceAttribute(15, 0));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                               {
                                       Icon = SdfIconType.ArrowRepeat,
                               });
            }
            
            if (member.Name == "ShowDB")
            {
                attributes.Add(new HorizontalGroupAttribute("Editor Functions"));
                attributes.Add(new PropertySpaceAttribute(15, 0));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                               {
                                       Icon = SdfIconType.ListColumnsReverse,
                               });
            }
        }
    }
#endif
}
