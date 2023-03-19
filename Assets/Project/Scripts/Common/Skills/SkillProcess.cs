using System;
using Common.Characters;
using UnityEngine;

namespace Common.Skills
{
    public class SkillProcess : MonoBehaviour, IEditable
    {
        [SerializeField] private float processTIme;

        private float ProcessWeight { get; set; }
        private Action complete;
        private CharacterBehaviour Cb { get; set; }

        public FloatEvent CastingProgress { get; } = new(0, float.MaxValue);
        public float ProcessTime => processTIme;
        

        private void StartProcessing(float hasteWeight)
        {
            ProcessWeight = processTIme * CharacterUtility.GetHasteValue(hasteWeight);
            enabled       = true;
        }

        private void StopProcessing()
        {
            CastingProgress.Value = 0f;
            enabled               = false;
        }

        private void Awake()
        {
            CastingProgress.Value = 0f;
            ProcessWeight         = processTIme;
            enabled               = false;

            if (!TryGetComponent(out SkillComponent skill))
            {
                Debug.LogError($"Require SKillComponent. Call From:{gameObject.name}");
                return;
            }

            Cb       = skill.Cb;
            complete = skill.Complete;
            
            skill.OnActivated.Register("StartProcessing", () => StartProcessing(Cb.StatTable.Haste));
            skill.OnCanceled.Register("CancelProcessing", StopProcessing);
            skill.OnEnded.Register("StopProcessing", StopProcessing);
        }

        private void Update()
        {
            if (CastingProgress.Value < ProcessWeight)
            {
                CastingProgress.Value += Time.deltaTime;
            }
            else
            {
                complete?.Invoke();
                StopProcessing();
            }
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!TryGetComponent(out SkillComponent skill))
            {
                Debug.LogError($"Require SKillComponent. Call From:{gameObject.name}");
                return;
            }
            
            var skillData = Database.SkillSheetData(skill.ActionCode);
            
            processTIme = skillData.ProcessTime;
        }
#endif
    }
}
