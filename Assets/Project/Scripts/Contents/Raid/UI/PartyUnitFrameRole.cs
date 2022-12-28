using System.Linq;
using Sirenix.OdinInspector;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI
{
    public class PartyUnitFrameRole : MonoBehaviour
    {
        [SerializeField] private PartyUnitFrame unitFrame;
        [SerializeField] private Image tank;
        [SerializeField] private Image dps;
        [SerializeField] private Image heal;

        private int instanceID;
        
        private void SetRoleIcon(string role)
        {
            switch (role)
            {
                case "Tank" : tank.enabled = true;
                    return;
                case "Melee" or "Range" : dps.enabled = true;
                    return;
                case "Healer" : heal.enabled = true;
                    return;
                default: Debug.LogError($"role is not exist. input : {role}");
                    return;
            }
        }

        private void Awake()
        {
            instanceID = GetInstanceID();
            unitFrame ??= GetComponentInParent<PartyUnitFrame>();
            unitFrame.OnInitialize.Register(instanceID, 
                () => SetRoleIcon(unitFrame.AdventurerBehaviour.Role));
            
            if (tank.enabled) tank.enabled = false;
            if (dps.enabled) dps.enabled = false;
            if (heal.enabled) heal.enabled = false;
        }

        private void OnDestroy()
        {
            unitFrame.OnInitialize.Unregister(instanceID);
        }

#if UNITY_EDITOR
        [Button]
        private void SetUp()
        {
            unitFrame ??= GetComponentInParent<PartyUnitFrame>();
            tank ??= gameObject.Children().First(x => x.name == "Tank").GetComponent<Image>();
            dps ??= gameObject.Children().First(x => x.name == "Dps").GetComponent<Image>();
            heal ??= gameObject.Children().First(x => x.name == "Heal").GetComponent<Image>();
        }
#endif
    }
}
