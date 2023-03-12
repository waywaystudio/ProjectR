using Common;
using Sirenix.OdinInspector;
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

        private void SetRoleIcon(RoleType role)
        {
            switch (role)
            {
                case RoleType.Tank : tank.enabled = true;
                    return;
                case RoleType.Melee : dps.enabled = true;
                    return;
                case RoleType.Range : dps.enabled = true;
                    return;
                case RoleType.Healer : heal.enabled = true;
                    return;
                default: Debug.LogError($"role is not exist. input : {role}");
                    return;
            }
        }

        private void Awake()
        {
            unitFrame ??= GetComponentInParent<PartyUnitFrame>();
            
            unitFrame.OnInitialize.Register("SetRoleIcon", 
                () => SetRoleIcon(unitFrame.adventurer.Role));
            
            if (tank.enabled) tank.enabled = false;
            if (dps.enabled) dps.enabled = false;
            if (heal.enabled) heal.enabled = false;
        }

        private void OnDestroy()
        {
            unitFrame.OnInitialize.Unregister("SetRoleIcon");
        }

#if UNITY_EDITOR
        [Button]
        private void SetUp()
        {
            unitFrame ??= GetComponentInParent<PartyUnitFrame>();
            // tank ??= gameObject.Children().First(x => x.name == "Tank").GetComponent<Image>();
            // dps ??= gameObject.Children().First(x => x.name == "Dps").GetComponent<Image>();
            // heal ??= gameObject.Children().First(x => x.name == "Heal").GetComponent<Image>();
        }
#endif
    }
}
