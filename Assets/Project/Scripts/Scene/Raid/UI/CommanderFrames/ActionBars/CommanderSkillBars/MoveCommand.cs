using System;
using System.Collections.Generic;
using Character;
using Core;
using MainGame;
using TMPro;
using UnityEngine;

namespace Raid.UI.CommanderFrames.ActionBars.CommanderSkillBars
{
    public class MoveCommand : MonoBehaviour
    {
        [SerializeField] private CommandActionIcon commandAction;
        [SerializeField] private TextMeshProUGUI hoyKey;
        [SerializeField] private BindingCode bindingCode;

        private List<AdventurerBehaviour> adventurersList;
        
        private string HotKey =>
            bindingCode switch
            {
                BindingCode.Q => "Q",
                BindingCode.W => "W",
                BindingCode.E => "E",
                BindingCode.R => "R",
                _ => "-",
            };

        public void Initialize(List<AdventurerBehaviour> adventurersList)
        {
            this.adventurersList = adventurersList;
            
            MainManager.Input.TryGetAction(bindingCode, out var inputAction);

            // 기존 캐릭터 스킬 액션 삭제.
            // inputAction.started -= 
            
            // Hot Key버튼을 클릭하면
            /// 마우스에 ToMoveCommand Decal 표시?
            // 마우스를 클릭하면
            /// 해당지점으로 모든 캐릭터가 이동.
            // adventurersList.ForEach(x => x.ActionBehaviour);
        }

        private void Awake()
        {
            commandAction ??= GetComponentInChildren<CommandActionIcon>();
            hoyKey.text   =   HotKey;
        }


        public void SetUp()
        {
            commandAction ??= GetComponentInChildren<CommandActionIcon>();
            hoyKey.text   =   HotKey;
        }
    }
}