using Core;
using Data.ContentData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class EntityAttribution : MonoBehaviour
    {
        public string SkillName = string.Empty;
        public EntityType Flag;
        
        public void Set<T>(T entityInfo) where T : class
        {
            entityInfo.CopyPropertiesTo(this as T);
        }

#if UNITY_EDITOR
        protected SkillData.Skill StaticData;
        [OnInspectorInit]
        protected virtual void OnEditorInitialize()
        {
            if (SkillName.IsNullOrEmpty()) return;

            Finder.TryGetObject(out SkillData skillData);
            StaticData = skillData.SkillList.Find(x => x.SkillName == SkillName);
        }
#endif
    }
}
