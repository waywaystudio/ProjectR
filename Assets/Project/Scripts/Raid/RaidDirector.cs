using System.Collections.Generic;
using Character;
using Character.Adventurers;
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
        public static Adventurer FocusCharacter => Instance.focusCharacter;
        public static Boss Boss => CastingDirector.Boss;
        public static List<Adventurer> AdventurerList => CastingDirector.AdventurerList;

        private Adventurer focusCharacter;
        

        public static void Initialize(List<DataIndex> adventurerEntry)
        {
            CastingDirector.Initialize(adventurerEntry);
            Instance.focusCharacter = CastingDirector.AdventurerList[0];
            
            UIDirector.Initialize();
            Instance.adventurerFocusEvent.Invoke(FocusCharacter);
        }

        
        protected override void Awake()
        {
            base.Awake();
            
            cameraDirector  ??= GetComponentInChildren<RaidCameraDirector>();
            castingDirector ??= GetComponentInChildren<RaidCastingDirector>();
            uiDirector      ??= GetComponentInChildren<RaidUIDirector>();

            MainUI.FadePanel.PlayFadeIn();
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
