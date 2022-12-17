using System;
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
        public override void TakeDamage(IDamageProvider damageInfo)
        {
            var hitChance = Random.Range(0f, 1.0f);
            
            if (hitChance > damageInfo.Hit)
            {
                // Debug.Log($"hitChance : {hitChance} hit : {damageInfo.Hit} Miss");
                return;
            }

            var damageAmount = 0d;

            if (Random.Range(0f, 1.0f) > damageInfo.Critical)
            {
                // Debug.Log("Critical!");
                damageAmount = damageInfo.CombatValue * 2d;
            }
            else
            {
                // Debug.Log("TakeDamage!");
                damageAmount = damageInfo.CombatValue;
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
        public override void TakeHeal(IHealProvider healInfo)
        {
            Debug.Log("TakeHeal!");
        }
        public override void TakeExtra(IExtraProvider extra)
        {
            Debug.Log("TakeExtra!");
        }
    }
}
