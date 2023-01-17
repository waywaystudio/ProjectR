using Core;
using UnityEngine;
// ReSharper disable MemberCanBeProtected.Global

namespace Character.Combat.Skill
{
    public abstract class SkillObject : CombatObject, ISkillInfo
    {
        [SerializeField] protected SkillTable skillTable;
        [SerializeField] protected float fixedCastingTime;
        [SerializeField] protected string animationKey;
        [SerializeField] protected int priority;
        [SerializeField] protected Sprite icon;

        public override ICombatProvider Provider => skillTable.Provider;

        public Sprite Icon => icon;
        public bool HasCastingModule => ModuleTable.ContainsKey(ModuleType.Casting);
        public float CastingTime => CastingModule.CastingTime;
        public float CastingProgress => CastingModule.CastingProgress;
        public bool HasCoolTimeModule => ModuleTable.ContainsKey(ModuleType.CoolTime);
        public float CoolTime => CoolTimeModule.OriginalCoolTime;
        public Observable<float> RemainTime => CoolTimeModule.RemainTime;

        public float FixedCastingTime => fixedCastingTime;
        public string AnimationKey => animationKey;
        public int Priority => priority;
        public bool IsSkillFinished { get; set; }
        public bool IsSkillReady 
        {
            get
            {
                // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                foreach (var item in ReadyCheckList) if (!item.IsReady) return false;
                
                return true;
            }
        }

        protected virtual void OnAssigned()
        {
            
        }

        public override void Active()
        {
            IsSkillFinished = false;
            
            base.Active();
        }

        public override void Complete()
        {
            IsSkillFinished = true;
            
            base.Complete();
        }


        protected virtual void Awake()
        {
            OnAssigned();
        }
        

        public override void SetUp()
        {
            base.SetUp();

            var skillData = MainGame.MainData.GetSkill(actionCode);

            skillTable       ??= GetComponentInParent<SkillTable>();
            fixedCastingTime =   skillData.CastingTime;
            animationKey     =   skillData.AnimationKey;
            priority         =   skillData.Priority;

            GetComponents<CombatModule>().ForEach(x => ModuleUtility.SetSkillModule(skillData, x));
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
#if UNITY_EDITOR
        private void ShowDB()
        {
            UnityEditor.EditorUtility.OpenPropertyEditor
                (MainGame.MainData.DataList.Find(x => x.Index == 13));
        }
        private void Reset() => actionCode = GetType().Name.ToEnum<DataIndex>();
#endif
        
    }
}
