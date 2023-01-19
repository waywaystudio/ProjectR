using Character;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.Nameplates
{
    public class Nameplate : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private Image healthBar;
        [SerializeField] private Image castingBar;

        private int instanceID;
        
        [ShowInInspector]
        private AdventurerBehaviour ab;
        private Transform abTransform;
        private Camera mainCamera;

        public void Initialize(AdventurerBehaviour ab)
        {
            this.ab     = ab;
            abTransform = ab.transform;
            
            ab.DynamicStatEntry.Hp.Register(instanceID, FillHealthBar);
            FillHealthBar(ab.DynamicStatEntry.Hp.Value);
        }

        public void UpdatePlate()
        {
            if (abTransform.IsNullOrEmpty()) return;
            
            if (abTransform.hasChanged)
            {
                transform.position = mainCamera.WorldToScreenPoint(abTransform.position) + offset;
            }

            FillCastingBar();
        }
        
        
        private void FillHealthBar(float hp)
        {
            var normalHp = hp / ab.StatTable.MaxHp;

            healthBar.DOFillAmount(normalHp, 0.1f);
        }

        private void FillCastingBar()
        {
            var skillInfo = ab.SkillInfo;

            if (skillInfo is not { HasCastingModule: true })
            {
                if (castingBar.enabled) castingBar.enabled = false;
                return;
            }
            
            if (!castingBar.enabled) castingBar.enabled = true;
            castingBar.fillAmount = skillInfo.CastingProgress.Value / skillInfo.CastingTime;
        }

        private void Awake()
        {
            instanceID = GetInstanceID();
            mainCamera = Camera.main;
        }

        private void OnDisable()
        {
            if (!ab.IsNullOrEmpty())
            {
                ab.DynamicStatEntry.Hp.Unregister(instanceID);
            }
        }
    }
}
