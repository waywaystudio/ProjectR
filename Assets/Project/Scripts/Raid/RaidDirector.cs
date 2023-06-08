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

        public static RaidCastingDirector CastingDirector => Instance.castingDirector;
        public static RaidStageDirector StageDirector => Instance.stageDirector;
        public static RaidUIDirector UIDirector => Instance.uiDirector;
        public static VenturerBehaviour FocusCharacter => Instance.focusCharacter;
        public static VillainBehaviour Boss => CastingDirector.Villain;
        public static List<VenturerBehaviour> AdventurerList => CastingDirector.VenturerList;
        public static void ChangeFocusAdventurer(VenturerBehaviour ab) => Instance.adventurerFocusEvent.Invoke(ab);

        private VenturerBehaviour focusCharacter;
        

        public static void Initialize(List<DataIndex> adventurerEntry)
        {
            CastingDirector.Initialize(adventurerEntry);
            UIDirector.Initialize();
            
            Instance.focusCharacter = CastingDirector.VenturerList[0];
            Instance.adventurerFocusEvent.Invoke(FocusCharacter);
        }

        
        protected override void Awake()
        {
            base.Awake();
            
            cameraDirector  ??= GetComponentInChildren<RaidCameraDirector>();
            castingDirector ??= GetComponentInChildren<RaidCastingDirector>();
            uiDirector      ??= GetComponentInChildren<RaidUIDirector>();
            
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
