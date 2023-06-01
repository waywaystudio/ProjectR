using System.Collections.Generic;
using Common.UI;
using UnityEngine;

namespace Lobby.UI.Forge.Upgrades
{
    public class VenturerStatTableUI : MonoBehaviour, IEditable
    {
        [SerializeField] private List<StatInfoUI> statInfoUIList;

        public void OnReloadForge()
        {
            var focusVenturerData = LobbyDirector.UI.Forge.FocusVenturerData;
            
            statInfoUIList.ForEach(statInfo =>
            {
                var venturerStatTextValue = focusVenturerData.GetStatTextValue(statInfo.StatType);
                
                statInfo.SetValue(venturerStatTextValue);
            });
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(statInfoUIList);
        }
#endif
    }
}
