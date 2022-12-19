using Common.Character.Operation.Combating;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Character
{
    public class MonsterBehavior : CharacterBehaviour
    {
        protected override void Start()
        {
            base.Start();
            
            MoveSpeed.RegisterSumType("MB", 20f);
        }

        protected new void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
        
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            // ReSharper disable once PossibleNullReferenceException
            #region TEST FUNCTION

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;

            Run(hit.point);

            #endregion
        }

        private void OnDisable()
        {
            MoveSpeed.UnregisterSumType("MB");
        }

        public override GameObject Taker => gameObject;
        public override void TakeDamage(ICombatProvider combatInfo)
        {
            var hitChance = Random.Range(0f, 1.0f);
            
            if (hitChance > combatInfo.Hit)
            {
                // Debug.Log($"hitChance : {hitChance} hit : {damageInfo.Hit} Miss");
                return;
            }

            var damageAmount = 0d;

            if (Random.Range(0f, 1.0f) > combatInfo.Critical)
            {
                // Debug.Log("Critical!");
                damageAmount = combatInfo.CombatPower * 2d;
            }
            else
            {
                // Debug.Log("TakeDamage!");
                damageAmount = combatInfo.CombatPower;
            }
            
            Hp -= damageAmount;

            if (Hp <= 0.0d)
            {
                Debug.Log("Dead!");
                IsAlive = false;
            }
            
            Debug.Log("TakeDamage");
            // Debug.Log($"{damageAmount} from {damageInfo.Provider.name}!");
        }
        public override void TakeHeal(ICombatProvider healInfo)
        {
            Debug.Log("TakeHeal!");
        }
        public override void TakeStatusEffect(ICombatProvider statusEffect)
        {
            // var statusEffect = CombatManager.GenerateStatusEffect(statusEffect);
            // StatusEffectTable.Register(statusEffectInfo)
            
            Debug.Log("TakeExtra!");
        }

        public StatusEffect GenerateStatusEffect(ICombatProvider provider)
        {
            switch (provider.Name)
            {
                case "Corruption" : break;
                case "BloodDrain" : break;
            }

            return null;
        }
    }
}
