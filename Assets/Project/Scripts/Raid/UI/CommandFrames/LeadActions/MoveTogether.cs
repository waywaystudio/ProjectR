using Character.Venturers;
using Common;
using UnityEngine;

namespace Raid.UI.CommandFrames.LeadActions
{
    public class MoveTogether : MonoBehaviour
    {
        [SerializeField] private string bindingKey;
        
        public void OnCommandMode()
        {
            // Register First Action Slot to MoveToPoint
            RaidDirector.Input[bindingKey].ClearStart();
            RaidDirector.Input[bindingKey].AddStart("MoveToPoint", MoveToPoint);
        }

        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            
        }
        
        public static void MoveToPoint()
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
    }
}
