using UnityEditor;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class SkillData : MonoBehaviour
    {
        [SerializeField] private DataIndex actionCode;
        [SerializeField] private string skillName;
        [SerializeField] private float power;
        [SerializeField] private float coolTime;
        [SerializeField] private float castingTime; 
        [SerializeField] private float range;
        [SerializeField] private int priority;
        [SerializeField] private string animationKey;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        
        public DataIndex ActionCode => actionCode;
        public string SkillName => skillName;
        public float Power => power;
        public float CoolTime => coolTime;
        public float CastingTime => castingTime;
        public float Range => range;
        public int Priority => priority;
        public string AnimationKey => animationKey;
        public Sprite Icon => icon;
        public string Description => description;
        
        public void SetUp(DataIndex actionCode)
        {
            var skillData = MainGame.MainData.GetSkill(actionCode);
        
            this.actionCode = actionCode;
            skillName       = skillData.Name;
            power           = skillData.BaseValue;
            coolTime        = skillData.BaseCoolTime;
            castingTime     = skillData.CastingTime;
            range           = skillData.Range;
            priority        = skillData.Priority;
            animationKey    = skillData.AnimationKey;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
