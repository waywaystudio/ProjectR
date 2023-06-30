using System.Collections.Generic;
using Adventurers;
using Character.Venturers;
using Character.Villains;
using Singleton;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Raid
{
    public class RaidDirector : MonoSingleton<RaidDirector>, IEditable
    {
        [SerializeField] private RaidCameraDirector cameraDirector;
        [SerializeField] private RaidStageDirector stageDirector;
        [SerializeField] private RaidCastingDirector castingDirector;
        [SerializeField] private RaidUIDirector uiDirector;
        [SerializeField] private GameEventAdventurer adventurerFocusEvent;

        private VenturerBehaviour focusVenturer;
        
        public static RaidCastingDirector CastingDirector => Instance.castingDirector;
        public static RaidStageDirector StageDirector => Instance.stageDirector;
        public static RaidUIDirector UIDirector => Instance.uiDirector;
        public static VillainBehaviour Boss => CastingDirector.Villain;
        public static List<VenturerBehaviour> VenturerList => CastingDirector.VenturerList;
        public static VenturerBehaviour FocusVenturer
        {
            get => Instance.focusVenturer;
            set
            {
                Instance.focusVenturer = value;
                Instance.adventurerFocusEvent.Invoke(value);
            }
        }

        /*
         * Raid Start
         */ 
        public static void Initialize()
        {
            CastingDirector.Initialize();
            UIDirector.Initialize();
            
            FocusVenturer = CastingDirector.VenturerList[0];
        }

        
        protected override void Awake()
        {
            base.Awake();

            // TEMP
            uiDirector.gameObject.SetActive(true);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            cameraDirector  ??= GetComponentInChildren<RaidCameraDirector>();
            castingDirector ??= GetComponentInChildren<RaidCastingDirector>();
            uiDirector      ??= GetComponentInChildren<RaidUIDirector>();
            
            Instance.gameObject.GetComponentsInOnlyChildren<IEditable>().ForEach(editable => editable.EditorSetUp());
        }
#endif
    }
}
