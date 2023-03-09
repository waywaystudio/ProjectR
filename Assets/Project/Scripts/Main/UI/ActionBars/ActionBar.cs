using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MainGame.UI.ActionBars
{
    public class ActionBar : MonoBehaviour, IEditable
    {
        [SerializeField] protected string barName;
        [SerializeField] protected int slotCount;
        [SerializeField] protected List<ActionSlot> actionSlotList;


#if UNITY_EDITOR
        [Button]
        private void GenerateSlot()
        {
            //...
        }

        public void EditorSetUp()
        {
            GetComponentsInChildren(actionSlotList);
        }
#endif
        
    }
}
