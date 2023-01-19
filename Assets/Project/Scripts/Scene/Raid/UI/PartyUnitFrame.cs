using Character;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Raid.UI
{
    public class PartyUnitFrame : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image resourceBar;
        [SerializeField] private TextMeshProUGUI resourceText;
        [SerializeField] private TextMeshProUGUI nameText;
        
        private int instanceID;
        // private RaidUIDirector uiDirector;
        private bool isInitialized;

        public ActionTable OnInitialize { get; } = new();
        public AdventurerBehaviour AdventurerBehaviour { get; private set; }

        public void Initialize(AdventurerBehaviour ab)
        {
            AdventurerBehaviour = ab;
            
            // Set ResourceColor
            // Set RoleIcon
            nameText.text = ab.Name;
            // Set Avatar
            
            ab.DynamicStatEntry.Hp.Register(instanceID, FillHealthBar);
            ab.DynamicStatEntry.Resource.Register(instanceID, FillResourceBar);

            FillHealthBar(ab.DynamicStatEntry.Hp.Value);
            FillResourceBar(ab.DynamicStatEntry.Resource.Value);
            
            OnInitialize.Invoke();
            isInitialized = true;
        }


        private void FillHealthBar(float hp)
        {
            var normalHp = hp / AdventurerBehaviour.StatTable.MaxHp;

            healthBar.DOFillAmount(normalHp, 0.1f);
            healthText.text = hp.ToString("N0");
        }

        private void FillResourceBar(float resource)
        {
            var normalResource = resource / AdventurerBehaviour.StatTable.MaxResource;

            resourceBar.DOFillAmount(normalResource, 0.1f);
            resourceText.text = resource.ToString("N0");
        }

        private void Awake()
        {
            instanceID = GetInstanceID();
            // uiDirector = GetComponentInParent<RaidUIDirector>();
            // uiDirector.PartyFrameList.Add(this);
        }

        private void OnDisable()
        {
            if (isInitialized)
            {
                AdventurerBehaviour.DynamicStatEntry.Hp.Unregister(instanceID);
                AdventurerBehaviour.DynamicStatEntry.Resource.Unregister(instanceID);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // uiDirector.FocusAdventurer = AdventurerBehaviour;
        }
    }
}
