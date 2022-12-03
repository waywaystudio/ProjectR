using System.Collections.Generic;
using System.Linq;
using Common.Character.Skills.Entity;
using Core;
using UnityEngine;

namespace Common.Character.Skills.Core
{
    public class SkillAttribution : MonoBehaviour
    {
        [SerializeField] protected int id;
        [SerializeField] protected string skillName;
        [SerializeField] protected string animationKey;
        [SerializeField] protected int priority;
        [SerializeField] private EntityType entityTypeList;
        
        private List<EntityAttribution> skillEntities = new();
        private List<IReadyRequired> readyRequiredSkillEntities;
        private List<IUpdateRequired> updateRequiredSkillEntities;
        
        public int ID { get => id; set => id = value; }
        public string SkillName { get => skillName; set => skillName = value; }
        public string AnimationKey { get => animationKey; set => animationKey = value; }
        public int Priority { get => priority; set => priority = value; }
        public bool IsReady => readyRequiredSkillEntities.All(x => x.IsReady);

        public void SetEntities(ICombatAttribution combatAttributionInfo)
        {
            skillEntities.ForEach(x => x.Set(combatAttributionInfo));
        }

        public void UpdateStatus()
        {
            if (updateRequiredSkillEntities.IsNullOrEmpty()) return;
            
            updateRequiredSkillEntities.ForEach(x => x.UpdateStatus());
        }

        protected virtual void Awake()
        {
            UpdateEntityType();

            skillEntities = GetComponents<EntityAttribution>().ToList();
            readyRequiredSkillEntities = GetComponents<IReadyRequired>().ToList();
            updateRequiredSkillEntities = GetComponents<IUpdateRequired>().ToList();
        }

        private void UpdateEntityType()
        {
            GetComponents<EntityAttribution>().ForEach(x => entityTypeList = entityTypeList.Include(x.Flag));
        }

#if UNITY_EDITOR
        #region EditorOnly
        [Sirenix.OdinInspector.OnInspectorInit]
        protected virtual void Initialize()
        {
            Finder.TryGetObject(out Data.ContentData.SkillData skillData);
        
            var staticData = skillData.SkillList.Find(x => x.SkillName.Equals(GetType().Name));
            if (staticData is null)
            {
                Debug.LogWarning($"Can't Find {skillName} in {skillData} ScriptableObject");
                return;
            }
        
            id = staticData.ID;
            skillName = staticData.SkillName;
            priority = staticData.Priority;
            animationKey = staticData.MotionType;
            
            GetComponents<EntityAttribution>().ForEach(x =>
            {
                if (x.SkillName != this.GetType().Name)
                    x.SkillName = GetType().Name;
            });

            UpdateEntityType();
        }
        #endregion
#endif

        // public virtual string ExtraStatus { get; protected set; } = string.Empty;

        // 사용 전 갱신함수
        // public void CommonAttack()
        // {
        //      if (!CombatManager.TrySetValueEntity(characterData, ref commonAttack) return;
        //      CombatManager.Damage(Skill, FocusTarget);
        // }
        // public bool TrySetSkillValues(IDamageInfo damageInfo, ref Skill refreshedSkillOrigin);
        
        // 방향, 거리 등 추가적으로 데미지계산에 관련된 스킬도 있을 수 있다.
        // 이 값들은 TakeDamage 호출전에 Value에 합산되어도 되어야 한다.
        // public void Damage(IDamage Skill, GameObject Target)
        // => CombatManager.Damage(Skill, Target);
        // public void TakeDamage(IDamage Skill)
        // => CombatManager.TakeDamage(self, Skill);
        // public void TakeHeal(IHeal Skill)
        // => CombatManager.TakeHeal(self, Skill);
        
        // Buff & DeBuff Type
        // BUff
        // public double Value : 뭔가 올려줄테니까 값이 있어야 한다.
        // public float Duration : 지속시간이 있겠죠
        // public string(==Key) BuffName : BuffData에서 Key값으로 찾아서 대상에게 적용한다.
        // public void Buff(IBuff Skill, ITakeBuff Target) // 동사 + 목적어가 자연스러운데...
        // => CombatManager.Buff(Skill, Target);
        
        // 사용 전 갱신함수
        // public void Protection()
        // {
        //      if (!CombatManager.TrySetBuffEntity(characterData, ref protection) return;
        //      CombatManager.Buff(Skill, FocusTarget);
        // }
        // 자 일단 데미지만 해보자...
    }
}
