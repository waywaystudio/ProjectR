using System.Collections.Generic;
using Character;
using Core;
using Core.Singleton;
using MainGame;
using UnityEngine;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Raid
{
    /* Raid는 프리팹 베리언트로 만들어질 예정.
     * 각 레이드 마다 프리팹이 존재 할 것이며, 보스 몬스터, 스테이지를 기본 포함한다.
     * 다음 항목을 다른 씬에서 받는다.
     * 플레이어
     * 파티원
     * 설정 난이도.
     */
    public class RaidDirector : MonoSingleton<RaidDirector>, IEditable
    {
        [SerializeField] private RaidCameraDirector cameraDirector;
        [SerializeField] private RaidCastingDirector castingDirector;
        [SerializeField] private RaidUIDirector uiDirector;

        [SerializeField] private GameEventAdventurer OnFocusedAdventurerChanged;

        // TODO. TEMP;
        [SerializeField] private AdventurerBehaviour player;
        [SerializeField] private MonsterBehaviour boss;
        
        private AdventurerBehaviour focusCharacter;

        public static RaidCastingDirector CastingDirector =>
            Instance.castingDirector ??= Instance.GetComponentInChildren<RaidCastingDirector>();
        public static RaidUIDirector UIDirector => 
            Instance.uiDirector ??= Instance.GetComponentInChildren<RaidUIDirector>();

        public static AdventurerBehaviour Player => Instance.player;
        public static MonsterBehaviour Boss => Instance.boss;
        public static List<AdventurerBehaviour> AdventurerList => CastingDirector.AdventurerList;
        public static List<MonsterBehaviour> MonsterList => CastingDirector.MonsterList;

        public static AdventurerBehaviour FocusCharacter
        {
            get => Instance.focusCharacter;
            set
            {
                Instance.focusCharacter = value;
                Instance.OnFocusedAdventurerChanged.Invoke(value);
            }
        }


        /* TODO. FUTURE...
        public void Initialize(AdventurerBehaviour player, List<AdventurerBehaviour> party, int level)
        {
            this.player = player;
            castingDirector.SetCharacter(party, level);
             
            cameraDirector.SetUp();
            castingDirector.SetUp();
            ui.SetUp();
             
            MainUI.FadePanel.PlayFadeIn();
        }*/

        // TODO. 개발 버전에서는 Initialize를 대체한다.
        protected override void Awake()
        {
            base.Awake();
            
            cameraDirector  ??= GetComponentInChildren<RaidCameraDirector>();
            castingDirector ??= GetComponentInChildren<RaidCastingDirector>();
            uiDirector      ??= GetComponentInChildren<RaidUIDirector>();
            focusCharacter = player;

            MainUI.FadePanel.PlayFadeIn();
        }

        // TODO. Character의 Awake후에 들어와야 한다.
        private void Start()
        {
            uiDirector.Initialize();
            OnFocusedAdventurerChanged.Invoke(Player);
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
