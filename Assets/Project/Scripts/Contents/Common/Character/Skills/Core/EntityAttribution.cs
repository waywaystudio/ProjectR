using Common.Character.Skills.Entity;
using Core;
using Skill = MainGame.Data.ContentData.SkillData.Skill;
using UnityEngine;

namespace Common.Character.Skills.Core
{
    public abstract class EntityAttribution : MonoBehaviour
    {
        [SerializeField] protected SkillAttribution skill;
        [SerializeField] protected EntityType flag;

        public abstract bool IsReady { get; }

        protected SkillAttribution Skill => skill ??= GetComponent<SkillAttribution>();
        protected Skill SkillData => Skill.SkillData;
        protected CharacterBehaviour Cb => Skill.Combat.Cb;
        protected EntityType Flag { get => flag; set => flag = value; }


        protected abstract void SetEntity();
        protected virtual void Awake()
        {
            if (!Skill.EntityTable.ContainsKey(flag))
                Skill.EntityTable.Add(flag, this);
        }

#if UNITY_EDITOR
        [Sirenix.OdinInspector.OnInspectorInit]
        protected abstract void OnEditorInitialize();
#endif
    }
}
