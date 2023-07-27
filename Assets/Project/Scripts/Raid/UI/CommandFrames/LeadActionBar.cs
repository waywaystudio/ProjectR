using Common;
using UnityEngine;

namespace Raid.UI.CommandFrames
{
    public class LeadActionBar : MonoBehaviour, IEditable
    {
        [SerializeField] private LayerMask targetLayerMask;
        [SerializeField] private string moveBindingKey = "Q";
        [SerializeField] private string targetingBindingKey = "W";
        [SerializeField] private GameObject moveTogetherAction;
        [SerializeField] private GameObject targetingTogetherAction;

        private readonly Collider[] forceTargetBuffer = new Collider[32];
        
        
        public void OnCommandModeEnter()
        {
            RaidDirector.Input[moveBindingKey].AddStart("MoveToPoint", MoveToPoint);
            RaidDirector.Input[targetingBindingKey].AddStart("ForceTargeting", ForceTargeting);
            
            moveTogetherAction.SetActive(true);
            targetingTogetherAction.SetActive(true);
        }
        
        public void OnCommandModeExit()
        {
            RaidDirector.Input[moveBindingKey].RemoveStart("MoveToPoint");
            RaidDirector.Input[targetingBindingKey].RemoveStart("ForceTargeting");
            
            moveTogetherAction.SetActive(false);
            targetingTogetherAction.SetActive(false);
            
            ReleaseRigidity();
        }
        
        
        private static void MoveToPoint()
        {
            var groundPosition = InputManager.GetMousePosition();

            RaidDirector.VenturerList.ForEach(venturer =>
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

        private void ForceTargeting()
        {
            var mousePosition = InputManager.GetMousePosition();
            var takers = TargetUtility.GetTargetsInSphere<ICombatTaker>(mousePosition, targetLayerMask, 1f, forceTargetBuffer);

            if (takers.IsNullOrEmpty()) return;
            
            takers.Sort(mousePosition, SortingType.DistanceAscending);

            RaidDirector.VenturerList.ForEach(venturer =>
            {
                if (venturer.CombatClass == CharacterMask.Knight) return;
                if (venturer.IsRigid) venturer.IsRigid = false;
                
                venturer.ForceTargeting(takers[0]);
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
            moveTogetherAction      = transform.Find("MoveTogether").gameObject;
            targetingTogetherAction = transform.Find("FocusTarget").gameObject;
        }
#endif
    }
}
