using System.Collections;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Actions
{
    public class GlobalCoolDown : MonoBehaviour
    {
        [ShowInInspector] private const float GlobalCoolTime = 1.2f;

        private Coroutine gcdRoutine;
        
        private ICombatProvider Provider { get; set; }
        public FloatEvent GlobalCoolTimeProgress { get; } = new(0, float.MaxValue);
        public bool IsCooling => GlobalCoolTimeProgress.Value != 0f;


        public void StartCooling()
        {
            if (gcdRoutine != null) StopCoroutine(gcdRoutine);
            gcdRoutine = StartCoroutine(Cooling());
        }


        private IEnumerator Cooling()
        {
            var hastedCoolTime = GlobalCoolTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);
            
            GlobalCoolTimeProgress.Value = hastedCoolTime;

            while (GlobalCoolTimeProgress.Value > 0)
            {
                GlobalCoolTimeProgress.Value -= Time.deltaTime;
                yield return null;
            }
        }
        
        private void Awake()
        {
            Provider = GetComponentInParent<ICombatProvider>();
        }
    }
}
