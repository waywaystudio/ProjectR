using System.Threading;
using Character.Venturers;
using Common;
using Common.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.VenturerFrames
{
    public class VenturerSkillSlot : MonoBehaviour, IEditable
    {
        [SerializeField] private string bindingKey;
        [SerializeField] private Image skillIcon;
        [SerializeField] private ImageFiller coolDownFiller;
        [SerializeField] private TextMeshProUGUI hotKey;

        private static VenturerBehaviour FocusVenturer => RaidDirector.FocusVenturer;
        private string HotKey => bindingKey;
        private DataIndex SkillCode { get; set; } = DataIndex.None;
        private CancellationTokenSource cts;


        public void Initialize()
        {
            RaidDirector.Input[bindingKey]?.ClearStart();
            RaidDirector.Input[bindingKey]?.ClearCancel();
            RaidDirector.Input[bindingKey]?.AddStart("ActiveSkill", StartAction);
            RaidDirector.Input[bindingKey]?.AddCancel("ReleaseSkill", ReleaseAction);
        }

        public void UpdateSkill(DataIndex skillCode)
        {
            if (SkillCode == skillCode) return;
            
            var targetSkill = FocusVenturer.SkillTable[skillCode];
            var builder = new CombatSequenceBuilder(targetSkill.Sequence);

            SkillCode        = skillCode;
            skillIcon.sprite = targetSkill.Icon;

            builder
                .Add(Section.Cancel, "CancelReservation", StopTrying);
            
            coolDownFiller.RegisterTrigger(targetSkill.CoolTimer);
        }


        private void StartAction(InputAction.CallbackContext callbackContext)
        {
            if (!InputManager.TryGetMousePosition(out var mousePosition)) return;

            TryActiveSkill(mousePosition).Forget();
        }

        private void ReleaseAction(InputAction.CallbackContext callbackContext)
        {
            var skill = FocusVenturer.SkillTable[SkillCode];

            if (!Verify.IsNotNull(skill)) return;
            
            skill.Invoker.Release();
        }

        private async UniTaskVoid TryActiveSkill(Vector3 mousePosition)
        {
            StopTrying();
            
            cts = new CancellationTokenSource();
            
            var targetSkill = FocusVenturer.SkillTable[SkillCode];
            var tryTimer = 0f;

            while (!targetSkill.Invoker.IsAbleToActive && tryTimer < 0.5f)
            {
                FocusVenturer.ActiveSkill(SkillCode, mousePosition);
                tryTimer += Time.deltaTime;

                await UniTask.Yield(cts.Token);
            }
            
            FocusVenturer.ActiveSkill(SkillCode, mousePosition);
        }

        private void StopTrying()
        {
            if (cts == null) return;
            
            cts.Cancel();
            cts = null;
        }

        private void OnDestroy()
        {
            StopTrying();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            hotKey         = transform.Find("Hotkey").GetComponent<TextMeshProUGUI>();
            skillIcon      = transform.Find("Icon").GetComponent<Image>();
            coolDownFiller = transform.Find("Cooldown").GetComponent<ImageFiller>();
            hotKey.text    = HotKey;
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
