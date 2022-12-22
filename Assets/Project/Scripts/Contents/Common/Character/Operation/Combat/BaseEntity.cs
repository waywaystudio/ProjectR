using UnityEngine;
using SkillData = MainGame.Data.ContentData.SkillData.Skill;

namespace Common.Character.Operation.Combat
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField] protected EntityType flag;
        
        protected int InstanceID;
        protected CharacterBehaviour Cb;
        protected BaseSkill AssignedSkill;

        public abstract bool IsReady { get; }
        public abstract void SetEntity();

        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
            Cb = GetComponentInParent<CharacterBehaviour>();
            AssignedSkill = GetComponent<BaseSkill>();
            AssignedSkill.EntityTable.TryAdd(flag, this);
        }
    }
}
