using UnityEngine;
using SkillData = MainGame.Data.ContentData.SkillData.Skill;

namespace Common.Character.Operation.Combating
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField] protected EntityType flag;
        [SerializeField] private BaseSkill skill;
        protected int InstanceID;
        
        protected BaseSkill Skill => skill ??= GetComponent<BaseSkill>();
        protected SkillData SkillData => Skill.SkillData;
        protected CharacterBehaviour Cb => Skill.Combat.Cb;

        public abstract bool IsReady { get; }
        public abstract void SetEntity();

        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
            
            skill ??= GetComponent<BaseSkill>();
            Skill.EntityTable.TryAdd(flag, this);
        }
    }
}
