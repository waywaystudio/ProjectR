using Character.StatusEffect;
using Core;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Character.Skill.Knight
{
    public class CastingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        [SerializeField] private GameObject bleedPrefab;
        [SerializeField] private int maxPool = 8;
        
        private IObjectPool<StatusEffectComponent> pool;
        public StatTable StatTable { get; } = new();


        public override void Release() { }

        protected override void UpdateCompletion()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }

        protected override void PlayAnimation()
        {
            model.PlayLoop(animationKey);
        }
        
        private void OnCastingAttack()
        {
            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");
                
                var effectInfo = bleedPrefab.GetComponent<IStatusEffect>();
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

        private void PlayEndCastingAnimation()
        {
            model.PlayOnce("attack", 0f, OnEnded.Invoke);
        }
        
        private StatusEffectComponent CreateStatusEffect()
        {
            var statusEffect = Instantiate(bleedPrefab).GetComponent<StatusEffectComponent>();
             
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
            OnActivated.Register("PlayCastingAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            OnActivated.Register("StartProgress", () => StartProcess(OnCompleted.Invoke));
            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("CastingAttack", OnCastingAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
            
            OnEnded.Register("StopProgress", StopProcess);
            OnEnded.Register("Idle", model.Idle);
            
            OnInterrupted.Register("Log", () => Debug.Log("Interrupted!"));
        }
        

        public override void SetUp()
        {
            base.SetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            powerValue.Value = skillData.CompletionValueList[0];
        }
    }
}
