using System.Collections;
using Core;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Character.Combat
{
    public class GlobalCoolDown : MonoBehaviour
    {
        // TODO. 캐릭터마다 다를 수 있다. 그렇지 않다면 Config를 만들어보자.
        [SerializeField] private float baseCoolTime = 2.0f;

        // SharedBool :: CombatBehaviorDesigner
        public bool IsCooling { get; set; }
        public Observable<float> Timer { get; } = new();
        public float CoolTime => baseCoolTime * CharacterUtility.GetHasteValue(ReferenceStatEntry.Haste);

        private Coroutine RoutineBuffer { get; set; }
        private StatTable ReferenceStatEntry { get; set; }
        

        public void StartCooling()
        {
            if (RoutineBuffer != null) StopCoroutine(RoutineBuffer);
            RoutineBuffer = StartCoroutine(Cooling());
        }
        

        private IEnumerator Cooling()
        {
            Timer.Value = CoolTime;
            
            IsCooling = true;

            while (Timer.Value > 0)
            {
                Timer.Value -= Time.deltaTime;
                yield return null;
            }

            IsCooling = false;
        }

        private void Awake()
        {
            ReferenceStatEntry = GetComponentInParent<CharacterBehaviour>().StatTable;
        }
    }
}
