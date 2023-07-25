using System.Collections.Generic;
using Character.Venturers;
using Character.Villains;
using Common;
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
        [SerializeField] private GameEvent onCommandModeEnter;
        [SerializeField] private GameEvent onCommandModeExit;

        private VenturerBehaviour focusVenturer;
        private VenturerBehaviour lastFocusVenturer;
        private static bool OnCommandMode { get; set; }

        public static RaidCameraDirector Camera => Instance.cameraDirector;
        public static RaidCastingDirector Casting => Instance.castingDirector;
        public static RaidInputDirector Input => Instance.inputDirector;
        public static RaidStageDirector Stage => Instance.stageDirector;
        public static RaidUIDirector UIDirector => Instance.uiDirector;
        public static List<VenturerBehaviour> VenturerList => Casting.VenturerList;
        public static VenturerBehaviour FocusVenturer
        {
            get => Instance.focusVenturer;
            set
            {
                if (value == Instance.focusVenturer)
                {
                    return;
                }
                
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

            FocusVenturer = VenturerList.Find(venturer => venturer.CombatClass == CharacterMask.Warrior);
            //[0];
        }

        public static void CommandMode()
        {
            if (OnCommandMode)
            {
                Instance.onCommandModeExit.Invoke();
                FocusVenturer = Instance.lastFocusVenturer;
            }
            else
            {
                Instance.lastFocusVenturer = FocusVenturer;
                FocusVenturer              = null;
                
                Instance.onCommandModeEnter.Invoke();
            }

            OnCommandMode = !OnCommandMode;
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
