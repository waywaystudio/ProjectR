using Common.Character.Skills.Core;
using Core;
using UnityEngine;

namespace Common.Character.Skills
{
    public class CommonAttack : BaseSkill, IHasCoolDown, IHasRange
    {
        // FromParent Field
        // # int id;
        // # string skillName;
        // # int priority;
        // # string motionType;
        [SerializeField] private float coolTime;
        [SerializeField] private float range;
        [SerializeField] private DamageEntity damageEntity = new();

        public IDamageEntity DamageEntity => damageEntity;
        
        private float remainCoolTime;

        public override bool IsReady => IsCoolOn;
        public bool IsCoolOn => RemainCoolTime <= 0;
        public float CoolTime => coolTime;
        public float RemainCoolTime 
        { 
            get => remainCoolTime;
            set => remainCoolTime = Mathf.Max(0, value);
        }

        public float Range => range;
        public float Distance { get; private set; }
        public bool IsInRange(Transform target) => Vector3.Distance(target.position, transform.position) <= Range;
        public void DecreaseCoolDown(float tick) => RemainCoolTime -= Time.deltaTime;
        
        public void SkillSetting(Transform target, double originalValue,
                                                   float critical,
                                                   float hitChance)
        {
            if (!IsReady) return;

            Value = originalValue * 1.2f;
            Critical = critical;
            HitChance = hitChance;
            remainCoolTime = coolTime;
            Distance = Vector3.Distance(target.position, transform.position);
        }

#region EditorOnly
#if UNITY_EDITOR
        [Sirenix.OdinInspector.OnInspectorInit]
        protected override void Initialize()
        {
            Finder.TryGetObject(out Data.ContentData.SkillData skillData);
        
            var staticData = skillData.SkillList.Find(x => x.SkillName.Equals(nameof(CommonAttack).ToPascalCase()));
            if (staticData is null)
            {
                Debug.LogWarning($"Can't Find {skillName} in {skillData} ScriptableObject");
                return;
            }
        
            id = staticData.ID;
            skillName = staticData.SkillName;
            coolTime = staticData.BaseCoolTime;
            range = staticData.Range;
            priority = staticData.Priority;
            motionType = staticData.MotionType;
        }
#endif
#endregion
    }
}
