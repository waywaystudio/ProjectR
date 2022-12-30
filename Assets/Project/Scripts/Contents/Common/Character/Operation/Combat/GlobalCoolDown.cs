using System.Collections;
using Core;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Common.Character.Operation.Combat
{
    public class GlobalCoolDown : MonoBehaviour
    {
        // TODO. 캐릭터마다 다를 수 있다. 그렇지 않다면 Config를 만들어보자.
        [SerializeField] private float baseCoolTime = 2.0f;
        
        private Coroutine globalCoolDownRoutine;
        private float timer;
        
        // SharedBool :: CombatBehaviorDesigner
        public bool IsCooling { get; set; }
        public float CoolTime => baseCoolTime * CharacterUtility.GetHasteValue(ReferenceStat.Haste);
        public ActionTable<float> OnTimerChanged { get; } = new();
        public float Timer
        {
            get => timer;
            set
            {
                timer = value;
                OnTimerChanged?.Invoke(value);
            }
        }
        
        private StatTable ReferenceStat { get; set; }
        

        public void CoolOn()
        {
            if (globalCoolDownRoutine != null) StopCoroutine(globalCoolDownRoutine);
            globalCoolDownRoutine = StartCoroutine(GlobalCoolDownRoutine());
        }
        

        private IEnumerator GlobalCoolDownRoutine()
        {
            Timer = CoolTime;
            
            IsCooling = true;

            while (Timer > 0)
            {
                Timer -= Time.deltaTime;
                yield return null;
            }

            IsCooling = false;
        }

        private void Awake()
        {
            ReferenceStat = GetComponentInParent<CharacterBehaviour>().StatTable;
        }
    }
}
