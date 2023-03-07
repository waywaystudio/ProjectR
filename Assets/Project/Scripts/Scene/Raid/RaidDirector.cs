using System.Collections.Generic;
using Character;
using Core;
using Core.Singleton;
using MainGame;
using UnityEngine;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Raid
{
    public class RaidDirector : MonoSingleton<RaidDirector>, IEditable
    {
        // TODO.TEMP : 이 후에는 로비씬에서 받아올 듯.
        [SerializeField] private List<DataIndex> adventurerEntry;
        [SerializeField] private DataIndex bossEntry;
        // 
        
        [SerializeField] private RaidCameraDirector cameraDirector;
        [SerializeField] private RaidStageDirector stageDirector;
        [SerializeField] private RaidCastingDirector castingDirector;
        [SerializeField] private RaidUIDirector uiDirector;
        [SerializeField] private GameEventAdventurer adventurerFocusEvent;

        private Adventurer focusCharacter;

        public static RaidCastingDirector CastingDirector =>
            Instance.castingDirector ??= Instance.GetComponentInChildren<RaidCastingDirector>();
        public static RaidStageDirector StageDirector =>
            Instance.stageDirector ??= Instance.GetComponentInChildren<RaidStageDirector>();
        public static RaidUIDirector UIDirector => 
            Instance.uiDirector ??= Instance.GetComponentInChildren<RaidUIDirector>();

        
        public static Adventurer FocusCharacter => Instance.focusCharacter;
        public static Boss Boss => MonsterList.IsNullOrEmpty() ? null : MonsterList[0];
        public static List<Adventurer> AdventurerList => CastingDirector.AdventurerList;
        public static List<Boss> MonsterList => CastingDirector.MonsterList;

        
        protected override void Awake()
        {
            base.Awake();
            
            cameraDirector  ??= GetComponentInChildren<RaidCameraDirector>();
            castingDirector ??= GetComponentInChildren<RaidCastingDirector>();
            uiDirector      ??= GetComponentInChildren<RaidUIDirector>();
            
            castingDirector.Initialize(adventurerEntry, bossEntry);
            focusCharacter = castingDirector.AdventurerList[0];
            MainUI.FadePanel.PlayFadeIn();
        }

        // TODO. Character의 Awake후에 들어와야 한다.
        private void Start()
        {
            uiDirector.Initialize();
            adventurerFocusEvent.Invoke(castingDirector.AdventurerList[0]);
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
