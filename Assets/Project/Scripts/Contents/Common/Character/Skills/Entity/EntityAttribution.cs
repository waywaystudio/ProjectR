using Common.Character.Skills.Core;
using Core;
using MainGame.Data.ContentData;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public abstract class EntityAttribution : MonoBehaviour
    {
        [SerializeField] protected SkillAttribution skill;
        [SerializeField] protected EntityType flag;

        public SkillAttribution Skill => skill ??= GetComponent<SkillAttribution>();
        protected SkillData.Skill SkillData => Skill.SkillData;
        protected string SkillName => Skill.SkillName;
        protected CharacterBehaviour Cb => Skill.Combat.Cb;
        public EntityType Flag { get => flag; protected set => flag = value; }

        public void Set<T>(T entityInfo) where T : class
        {
            entityInfo.CopyPropertiesTo(this as T);
        }
        
#if UNITY_EDITOR
        [Sirenix.OdinInspector.OnInspectorInit]
        protected virtual void OnEditorInitialize(){}
#endif
    }
}
