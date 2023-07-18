using System.Threading;
using Character.Venturers;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Raid.UI.CommandFrames
{
    public class LeadActionBar : MonoBehaviour, IEditable
    {
        [SerializeField] private string moveBindingKey;
        [SerializeField] private GameObject moveTogetherAction;

        private CancellationTokenSource cts;
        
        public void OnCommandModeEnter()
        {
            RaidDirector.Input[moveBindingKey].AddStart("MoveToPoint", MoveToPoint);
            
            moveTogetherAction.SetActive(true);
        }
        
        public void OnCommandModeExit()
        {
            RaidDirector.Input[moveBindingKey].RemoveStart("MoveToPoint");
            
            moveTogetherAction.SetActive(false);
            ReleaseRigidity();
        }
        
        
        private static void MoveToPoint()
        {
            var venturerList = RaidDirector.VenturerList;
            var groundPosition = InputManager.GetMousePosition();

            venturerList.ForEach(venturer =>
            {
                if (venturer.CombatClass == CharacterMask.Knight) return;

                venturer.IsRigid = true;
                var randomOffsetPosition 
                    = new Vector3(groundPosition.x + Random.Range(-2f, 2f), 
                                  groundPosition.y, 
                                  groundPosition.z + Random.Range(-2f, 2f));

                if (venturer.SkillTable.Current != null)
                    venturer.SkillTable.Current.Cancel();
                
                venturer.Run(randomOffsetPosition);
            });
        }

        private static void ReleaseRigidity()
        {
            var venturerList = RaidDirector.VenturerList;
            
            venturerList.ForEach(venturer =>
            {
                if (venturer.CombatClass == CharacterMask.Knight) return;

                venturer.IsRigid = false;
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
