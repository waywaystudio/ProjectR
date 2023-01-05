using System.Collections;
using Character.Combat.Entities;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable NotAccessedField.Local

namespace Character.Graphic.UI
{
    public class CastingBar : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private Image fillImage;

        private int instanceID;
        private float castingTime;
        private float castingProgress;
        private CastingEntity castingEntity;
        private Coroutine fillRoutine;


        private void UpdateCastingBar()
        {
            castingEntity = cb.CombatBehaviour.CurrentSkill.CastingEntity;

            if (castingEntity is null)
            {
                fillImage.fillAmount = 0f;
                return;
            }

            fillRoutine = StartCoroutine(FillProgress());
        }

        // NOTE. 캐스팅바에 OnValueChanged 구조로 가고 싶지만, 항상 캐스팅바가 존재하는게 아니다 보니까 약간 애매하다.
        // 값 자체를 복잡하게 받고 있다보니, if (castingEntity is null) break; 같은 구절이 필요하게 되었다.
        // 수정할 수 있으면 해보자.
        private IEnumerator FillProgress()
        {
            castingTime = castingEntity.CastingTime;
            castingProgress = 0f;
            fillImage.gameObject.SetActive(true);

            while (castingProgress < 1.0f)
            {
                if (castingEntity is null) break;
                
                castingProgress = castingEntity.CastingProgress / castingTime;
                fillImage.fillAmount = castingProgress;
                
                yield return null;
            }

            fillImage.gameObject.SetActive(false);
        }
    
        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            instanceID = GetInstanceID();
        }
    
        private void OnEnable() => cb.OnSkill.Register(instanceID, UpdateCastingBar);
        private void OnDisable() => cb.OnSkill.Unregister(instanceID);
    }
}
