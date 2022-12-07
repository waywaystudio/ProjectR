using System.Collections.Generic;
using System.Linq;
using Common.Character.Skills.Entity;
using Core;
using MainGame;
using MainGame.Data.ContentData;
using UnityEngine;

namespace Common.Character.Skills.Core
{
    public abstract class SkillAttribution : MonoBehaviour
    {
        [SerializeField] protected CharacterCombat combat;
        [SerializeField] protected int id;
        [SerializeField] protected string skillName;
        protected string animationKey;
        protected int priority;
        private EntityType entityTypeList;

        private SkillData.Skill skillData;
        private List<EntityAttribution> skillEntities = new();
        private List<IReadyRequired> readyRequiredEntities;
        private List<IUpdateRequired> updateRequiredEntities;

        public abstract List<ICombatTaker> TargetList { get; }
        public string SkillName => skillName ??= GetType().Name;
        public SkillData.Skill SkillData => skillData ??= MainData.GetSkillData(SkillName);
        public CharacterCombat Combat => combat ??= GetComponent<CharacterCombat>();
        protected CharacterBehaviour Cb => Combat.Cb;

        public string AnimationKey => "Attack"; // isTemporary, originally : animationKey;
        public int Priority { get => priority; set => priority = value; }
        private bool IsEntityReady => readyRequiredEntities.All(x => x.IsReady);

        public abstract void Invoke();

        public void UpdateStatus()
        {
            if (updateRequiredEntities.IsNullOrEmpty()) return;
            
            updateRequiredEntities.ForEach(x => x.UpdateStatus());
        }
        

        protected virtual void Awake()
        {
            UpdateEntityType();

            skillEntities = GetComponents<EntityAttribution>().ToList();
            readyRequiredEntities = GetComponents<IReadyRequired>().ToList();
            updateRequiredEntities = GetComponents<IUpdateRequired>().ToList();
        }

        protected bool TrySetEntities(ICombatAttribution combatInfo)
        {
            skillEntities.ForEach(x => x.Set(combatInfo));
            return IsEntityReady;
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
            id = SkillData.ID;
            priority = SkillData.Priority;
            animationKey = SkillData.AnimationKey;

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
