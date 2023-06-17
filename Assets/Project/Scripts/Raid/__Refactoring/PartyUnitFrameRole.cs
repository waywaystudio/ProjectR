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

        private void SetRoleIcon(CharacterMask role)
        {
            switch (role)
            {
                case CharacterMask.Knight : tank.enabled = true;
                    return;
                case CharacterMask.Rogue : dps.enabled = true;
                    return;
                case CharacterMask.Ranger : dps.enabled = true;
                    return;
                case CharacterMask.Priest : heal.enabled = true;
                    return;
                default: Debug.LogError($"role is not exist. input : {role}");
                    return;
            }
        }

        private void Awake()
        {
            unitFrame ??= GetComponentInParent<PartyUnitFrame>();
            
            unitFrame.OnInitialize.Add("SetRoleIcon", 
                () => SetRoleIcon(unitFrame.adventurer.CombatClass));
            
            if (tank.enabled) tank.enabled = false;
            if (dps.enabled) dps.enabled = false;
            if (heal.enabled) heal.enabled = false;
        }

        private void OnDestroy()
        {
            unitFrame.OnInitialize.Remove("SetRoleIcon");
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
