using System;
using Core;
using MainGame.UI.ActionBars;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame.UI.ActionIcon
{
    public class ActionIcon : MonoBehaviour, ITooltipInfo
    {
        [SerializeField] protected Image iconImage;

        private ActionBar assignedActionBar;
        private ActionSlot assignedActionSlot;
        
        public string TooltipInfo { get; set; }


        public void SetAssociation(ActionBar bar, ActionSlot slot)
        {
            assignedActionBar  = bar;
            assignedActionSlot = slot;
        }

        public void SetIcon(DataIndex actionCode)
        {
            
        }

        // public void SetAction(Action action) => this.action = action;

        public void SetPressAction() { }
        public void SetReleaseAction() { }
        
    }
}
