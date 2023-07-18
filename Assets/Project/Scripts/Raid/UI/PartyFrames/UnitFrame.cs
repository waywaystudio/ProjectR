using System;
using Character.Venturers;
using Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.PartyFrames
{
    public class UnitFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private Image symbol;
        [SerializeField] private ImageFiller healthFiller;
        [SerializeField] private ImageFiller resourceFiller;
        [SerializeField] private TextMeshProUGUI hpTextMesh;
        [SerializeField] private TextMeshProUGUI venturerNameMesh;

        private float hpCache = float.NegativeInfinity;

        public void Initialize(VenturerBehaviour vb)
        {
            var hpRef = vb.Hp;
            var maxHp = vb.StatTable.MaxHp;
            var resourceRef = vb.Resource;
            var maxResource = vb.StatTable.MaxResource;
            var lastSkillIndex = vb.SkillTable.SkillIndexList.LastOrDefault();


            symbol.sprite = vb.SkillTable[lastSkillIndex].Icon;
            healthFiller.RegisterEvent(hpRef, maxHp);
            resourceFiller.RegisterEvent(resourceRef, maxResource);
            
            hpRef.AddListener("HpToUnitFrameUI", UpdateVenturerHealth);
            venturerNameMesh.text = vb.Name;
        }


        private void UpdateVenturerHealth(float hp)
        {
            if (Math.Abs(hpCache - hp) < 0.000001f) return;
            
            hpTextMesh.text = hp.ToKmbt();
            hpCache         = hp;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            symbol           = transform.Find("Role").Find("Symbol").GetComponent<Image>();
            healthFiller     = transform.Find("HealthFill").GetComponent<ImageFiller>();
            resourceFiller   = transform.Find("ResourceFill").GetComponent<ImageFiller>();
            hpTextMesh       = transform.Find("HpValueText").GetComponent<TextMeshProUGUI>();
            venturerNameMesh = transform.Find("VenturerName").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}

/*
 * Annotation
 * Skeleton Graphic 초상화를 띄우려 했으나 Material쪽 문제로 실패.
 * [SerializeField] private AnimationUISkinController animationSkin;
 * animationSkin.Initialize(vb.Animating.SkinEntity);
 * animationSkin = transform.Find("Role").Find("Portrait").GetComponent<AnimationUISkinController>();
 */
