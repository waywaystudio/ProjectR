using Character.StatusEffect;
using Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.Skill.Knight
{
    public class GeneralAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        [SerializeField] private GameObject armorCrashPrefab;
        [SerializeField] private int maxPool = 8;
        
        private IObjectPool<StatusEffectComponent> pool;
        public StatTable StatTable { get; } = new();

        public override void Release() { }

        protected override void TryActiveSkill()
        {
            if (!ConditionTable.IsAllTrue) return;

            model.OnHit.Unregister("SkillHit");
            model.OnHit.Register("SkillHit", OnHit.Invoke);
            
            OnActivated.Invoke();
        }
        
        protected override void PlayAnimation()
        {
            model.PlayOnce(animationKey, progressTime,
                () =>
                {
                    OnCompleted.Invoke();
                    OnEnded.Invoke();
                });
        }

        protected override void UpdateCompletion()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }
        
        private StatusEffectComponent CreateStatusEffect()
        {
            var statusEffect = Instantiate(armorCrashPrefab).GetComponent<StatusEffectComponent>();
             
            statusEffect.SetPool(pool);

            return statusEffect;
        }

        private void OnStatusEffectGet(StatusEffectComponent statusEffect)
        {
            statusEffect.gameObject.SetActive(true);
            statusEffect.Initialize(Provider);
        }

        private static void OnStatusEffectRelease(StatusEffectComponent statusEffect)
        {
            statusEffect.gameObject.SetActive(false);
        }

        private static void OnStatusEffectDestroy(StatusEffectComponent statusEffect)
        {
            Destroy(statusEffect.gameObject);
        }

        private void OnGeneralAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");

                var effectInfo = armorCrashPrefab.GetComponent<IStatusEffect>();
                var table = taker.DynamicStatEntry.DeBuffTable;

                if (table.ContainsKey((Provider, effectInfo.ActionCode)))
                {
                    table[(Provider, effectInfo.ActionCode)].OnOverride();
                }
                else
                {
                    taker.TakeDeBuff(pool.Get());
                }
            });
        }
        
        protected override void Awake()
        {
            base.Awake();
                
            pool = new ObjectPool<StatusEffectComponent>(
                CreateStatusEffect,
                OnStatusEffectGet,
                OnStatusEffectRelease,
                OnStatusEffectDestroy,
                maxSize: maxPool);
        }

        protected void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            
            OnHit.Register("GeneralAttack", OnGeneralAttack);

            OnEnded.Register("ReleaseHit", () => model.OnHit.Unregister("SkillHit"));
            OnEnded.Register("Idle", model.Idle);
        }


        public override void SetUp()
        {
            base.SetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
