using Character.StatusEffect;
using Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.Skill.Knight
{
    public class ChargingAttack : SkillComponent, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        [SerializeField] private GameObject drainPrefab;
        [SerializeField] private int maxPool = 1;
        
        private IObjectPool<StatusEffectComponent> pool;
        public StatTable StatTable { get; } = new();


        protected override void PlayAnimation()
        {
            model.PlayLoop(animationKey);
        }
        
        private void UpdateCompletion()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }
        
        private void OnChargingAttack()
        {
            if (Provider.Object.TryGetComponent(out ICombatTaker self))
            {
                var effectInfo = drainPrefab.GetComponent<IStatusEffect>();
                var table = self.DynamicStatEntry.BuffTable;

                if (table.ContainsKey((Provider, effectInfo.ActionCode)))
                {
                    table[(Provider, effectInfo.ActionCode)].OnOverride();
                }
                else
                {
                    self.TakeBuff(pool.Get());
                }
            }

            if (!colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");
            });
        }

        private void PlayEndChargingAnimation()
        {
            model.PlayOnce("attack", 0f, OnEnded.Invoke);
        }
        
        private StatusEffectComponent CreateStatusEffect()
        {
            var statusEffect = Instantiate(drainPrefab).GetComponent<StatusEffectComponent>();
             
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


        protected void OnEnable()
        {
            pool = new ObjectPool<StatusEffectComponent>(
                CreateStatusEffect,
                OnStatusEffectGet,
                OnStatusEffectRelease,
                OnStatusEffectDestroy,
                maxSize: maxPool);
            
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdateCompletion);
            OnActivated.Register("StartProcess", () => StartProcess(OnCompleted.Invoke));
            
            OnInterrupted.Register("Log", () => Debug.Log("Interrupted!"));
            OnInterrupted.Register("EndCallback", OnEnded.Invoke);
            
            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("ChargingAttack", OnChargingAttack);
            OnCompleted.Register("PlayEndChargingAnimation", PlayEndChargingAnimation);
            OnCompleted.Register("StopProcess", StopProcess);
            OnCompleted.Register("ProgressEnd", () => OnProgress = false);

            OnEnded.Register("StopProgress", StopProcess);
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
