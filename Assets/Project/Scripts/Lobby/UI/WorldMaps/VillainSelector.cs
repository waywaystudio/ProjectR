using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.WorldMaps
{
    public class VillainSelector : MonoBehaviour, IEditable
    {
        [SerializeField] private VillainType villainType;
        [SerializeField] private Image villainSprite;

        private static VillainType FocusVillain => LobbyDirector.WorldMap.FocusVillain;

        public void OnButtonClicked()
        {
            LobbyDirector.WorldMap.FocusVillain = villainType;
            SelectedEffect();
        }

        public void OnFocusVillainUIChanged()
        {
            // Not targeted
            if (FocusVillain != villainType)
            {
                // Off HighLight
                DeSelectedEffect();
                return;
            }
        }


        private void SelectedEffect()
        {
            transform.DOScale(1.5f, 0.12f);
        }

        private void DeSelectedEffect()
        {
            transform.DOScale(1.0f, 0.05f);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            villainSprite        = transform.Find("Mask").Find("VillainSprite").GetComponent<Image>();
            villainSprite.sprite = Database.VillainSpriteData.Get((DataIndex)villainType);
            
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.PrefabUtility.ApplyPrefabInstance(gameObject, UnityEditor.InteractionMode.AutomatedAction);
        }
#endif
    }
}
