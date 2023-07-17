using System.Collections.Generic;
using Character.Venturers;
using Character.Villains;
using GameEvents;
using Singleton;
using UnityEngine;

namespace Raid
{
    public class RaidDirector : MonoSingleton<RaidDirector>, IEditable
    {
        [SerializeField] private RaidCameraDirector cameraDirector;
        [SerializeField] private RaidCastingDirector castingDirector;
        [SerializeField] private RaidInputDirector inputDirector;
        [SerializeField] private RaidStageDirector stageDirector;
        [SerializeField] private RaidUIDirector uiDirector;
        [SerializeField] private GameEventVenturer onFocusVenturerChanged;
        [SerializeField] private GameEvent onCommandMode;

        private VenturerBehaviour focusVenturer;

        public static RaidCameraDirector Camera => Instance.cameraDirector;
        public static RaidCastingDirector Casting => Instance.castingDirector;
        public static RaidInputDirector Input => Instance.inputDirector;
        public static RaidStageDirector Stage => Instance.stageDirector;
        public static RaidUIDirector UIDirector => Instance.uiDirector;
        public static VillainBehaviour Boss => Casting.Villain;
        public static List<VenturerBehaviour> VenturerList => Casting.VenturerList;
        public static VenturerBehaviour FocusVenturer
        {
            get => Instance.focusVenturer;
            set
            {
                if (value == Instance.focusVenturer) return;
                
                Instance.focusVenturer = value;
                Instance.onFocusVenturerChanged.Invoke(value);
            }
        }

        public static VillainBehaviour Villain => Casting.Villain;

        /*
         * Raid Start
         */ 
        public static void Initialize()
        {
            Camera.Initialize();
            Casting.Initialize();
            Input.Initialize();
            UIDirector.Initialize();
            
            FocusVenturer = Casting.VenturerList[0];
        }

        public static void CommandMode()
        {
            Instance.onCommandMode.Invoke();
            
            FocusVenturer = null;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            cameraDirector  ??= GetComponentInChildren<RaidCameraDirector>();
            castingDirector ??= GetComponentInChildren<RaidCastingDirector>();
            inputDirector   ??= GetComponentInChildren<RaidInputDirector>();
            stageDirector   ??= GetComponentInChildren<RaidStageDirector>();
            uiDirector      ??= GetComponentInChildren<RaidUIDirector>();
            
            Instance.gameObject.GetComponentsInOnlyChildren<IEditable>().ForEach(editable => editable.EditorSetUp());
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetSingleton()
        {
            if (!Instance.IsNullOrDestroyed())
                Instance.SetInstanceNull();
        }
#endif
    }
}
