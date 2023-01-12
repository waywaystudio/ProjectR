using System.Collections;
using Core;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Character.Combat
{
    public class GlobalCoolDown : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private float baseCoolTime = 2.0f;

        private Coroutine routineBuffer;
        private float tick;

        // SharedBool :: CombatBehaviorDesigner
        public bool IsCooling { get; set; }
        public Observable<float> Timer { get; } = new();
        public float CoolTime => baseCoolTime * CharacterUtility.GetHasteValue(cb.StatTable.Haste);
        

        public void StartCooling()
        {
            if (routineBuffer != null) StopCoroutine(routineBuffer);
            routineBuffer = StartCoroutine(Cooling());
        }
        

        private IEnumerator Cooling()
        {
            Timer.Value = CoolTime;
            
            IsCooling = true;

            while (Timer.Value > 0)
            {
                Timer.Value -= tick;
                yield return null;
            }

            IsCooling = false;
        }

        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>(); 
            tick = Time.deltaTime;
        }
    }
}
