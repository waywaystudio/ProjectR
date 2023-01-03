using System.Collections;
using Common.Character.Operation.Combat.Entity;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable NotAccessedField.Local

namespace Common.Character.Graphic.UI
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
            castingEntity = cb.CombatOperation.CurrentSkill.CastingEntity;

            if (castingEntity is null)
            {
                fillImage.fillAmount = 0f;
                return;
            }
            
            fillRoutine = StartCoroutine(FillProgress());
        }

        private IEnumerator FillProgress()
        {
            castingTime = castingEntity.CastingTime;
            castingProgress = 0f;
            fillImage.gameObject.SetActive(true);

            while (castingProgress < 1.0f)
            {
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
