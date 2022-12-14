using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Character
{
    public class MonsterBehavior : CharacterBehaviour
    {
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
        
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            // ReSharper disable once PossibleNullReferenceException
            #region TEST FUNCTION

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (!Physics.Raycast(ray, out var hit)) return;

            // Walk(hit.point);

            #endregion
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

            if (Random.Range(0f, 1.0f) > damageInfo.Critical)
            {
                // Debug.Log("Critical!");
                Hp -= damageInfo.CombatValue * 2d;
            }
            else
            {
                // Debug.Log("TakeDamage!");
                Hp -= damageInfo.CombatValue;
            }

            if (Hp <= 0.0d)
            {
                Debug.Log("Dead!");
                IsAlive = false;
            }
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
