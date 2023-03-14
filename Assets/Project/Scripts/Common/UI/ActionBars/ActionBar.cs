using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.UI.ActionBars
{
    public class ActionBar : MonoBehaviour, IEditable
    {
        [SerializeField] protected string barName;
        [SerializeField] protected int slotCount;
        [SerializeField] protected List<ActionSlot> actionSlotList;
        

        public void RegisterSlot(int slotIndex, IAssignable actionSymbol)
        {
            // if (actionSlotList.MoreThen(slotIndex))
            //     actionSlotList[slotIndex].RegisterSymbol(actionSymbol);
        }

        public void UnregisterSlot(int slotIndex)
        {
            // if (actionSlotList.MoreThen(slotIndex))
            //     actionSlotList[slotIndex].UnregisterSymbol();
        }


#if UNITY_EDITOR
        [Button]
        private void GenerateSlot()
        {
            //...
        }

        public void EditorSetUp()
        {
            GetComponentsInChildren(true, actionSlotList);
        }
#endif
        
    }
}
