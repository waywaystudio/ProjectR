using System.Collections;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Skill
{
    public class GlobalCoolDown : MonoBehaviour
    {
        [ShowInInspector] private const float GlobalCoolTime = 1.5f;

        private Coroutine gcdRoutine;
        private float gcdTick;
        
        private ICombatProvider Provider { get; set; }
        public FloatEvent GlobalCoolTimeProgress { get; } = new(0, float.MaxValue);
        public bool IsCooling
        { 
            get
            {
                if (GlobalCoolTimeProgress.Value != 0f) Debug.LogWarning("GlobalCoolDown On");

                return GlobalCoolTimeProgress.Value != 0f;
            }
        }


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
                GlobalCoolTimeProgress.Value -= gcdTick;
                yield return null;
            }
        }
        
        private void Awake()
        {
            Provider = GetComponentInParent<ICombatProvider>();
            gcdTick  = Time.deltaTime;
        }
    }
}
