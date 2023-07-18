using Character.Venturers;
using Common;
using UnityEngine;

namespace Raid.UI.CommandFrames
{
    public class LeadActionBar : MonoBehaviour, IEditable
    {
        [SerializeField] private string moveBindingKey;
        [SerializeField] private GameObject moveTogetherAction;
        
        public void OnCommandModeEnter()
        {
            // Register First Action Slot to MoveToPoint
            RaidDirector.Input[moveBindingKey].ClearStart();
            RaidDirector.Input[moveBindingKey].AddStart("MoveToPoint", MoveToPoint);
        }
        
        public void OnCommandModeExit()
        {
            // Register First Action Slot to MoveToPoint
            RaidDirector.Input[moveBindingKey].ClearStart();
            RaidDirector.Input[moveBindingKey].AddStart("MoveToPoint", MoveToPoint);
        }

        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            RaidDirector.Input[moveBindingKey].RemoveStart("MoveToPoint");
        }
        
        
        private static void MoveToPoint()
        {
            // Get All Dealer and Healer
            // First, Move one spot to all venturer. let's see.
            var venturerList = RaidDirector.VenturerList;
            var groundPosition = InputManager.GetMousePosition();
            
            venturerList.ForEach(venturer =>
            {
                if (venturer.CombatClass == CharacterMask.Knight) return;
                
                venturer.Run(groundPosition);
            });
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            moveTogetherAction = transform.Find("MoveTogetherAction").gameObject;
        }
#endif
    }
}
