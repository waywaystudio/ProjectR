using System.Collections.Generic;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Refinery
{
    public class VirtueInfoUI : MonoBehaviour, IEditable
    {
        [SerializeField] private EthosType virtueType;
        [SerializeField] private Slider deficiencyProgress;
        [SerializeField] private Slider excessProgress;
        [SerializeField] private List<ViceNode> deficiencyNodeList;
        [SerializeField] private List<ViceNode> excessNodeList;
        
        [ShowInInspector]
        private EthosType DeficiencyType => virtueType.GetSameThemeDeficiency();
        [ShowInInspector]
        private EthosType ExcessType => virtueType.GetSameThemeExcess();
        
        public EthosType VirtueType => virtueType;


        public void OnEthosChanged()
        {
            var deficiencyValue = LobbyDirector.Forge.VenturerEthosValue(DeficiencyType);
            var excessValue = LobbyDirector.Forge.VenturerEthosValue(ExcessType);

            deficiencyProgress.value = deficiencyValue;
            excessProgress.value     = excessValue;
            
            deficiencyNodeList.ForEach(node =>
            {
                if (deficiencyValue / 6 >= node.ChargeLevel) node.OnNode();
                else node.OffNode();
            });
            
            excessNodeList.ForEach(node =>
            {
                if (excessValue / 6 >= node.ChargeLevel) node.OnNode();
                else node.OffNode();
            });
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            deficiencyProgress = transform.Find("VirtueStatUI").Find("DeficiencySlider").GetComponent<Slider>();
            excessProgress     = transform.Find("VirtueStatUI").Find("ExcessSlider").GetComponent<Slider>();
            transform.Find("ViceNodeUI").Find("Deficiency").GetComponentsInChildren(deficiencyNodeList);
            transform.Find("ViceNodeUI").Find("Excess").GetComponentsInChildren(excessNodeList);

            if (!virtueType.IsVirtue())
            {
                Debug.LogWarning($"EthosType must be Virtue. Input:{virtueType}");
                return;
            }
            
            deficiencyNodeList.ForEach((node, index) =>
            {
                node.EditorViceNodeSetUp(DeficiencyType);
                node.ChargeLevel = index + 1;
            });
            excessNodeList.ForEach((node, index) =>
            {
                node.EditorViceNodeSetUp(ExcessType);
                node.ChargeLevel = index + 1;
            });
            
            OnEthosChanged();
            
            gameObject.GetComponentsInOnlyChildren<IEditable>().ForEach(component => component.EditorSetUp());
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
