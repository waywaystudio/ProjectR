using System;
using Common;
using Common.Characters;
using UnityEngine;

public interface IHasProjectorEntity
{
    FloatEvent Progress { get; }
    Vector2 SizeValue { get; set; }
}


namespace Common.Skills
{
    public class SkillCasting : MonoBehaviour, IEditable
    {
        [SerializeField] private float castingTime;

        private Action complete;
        
        public FloatEvent Progress { get; } = new();
        public float CastingTime => castingTime;
        
        private float CastingWeight { get; set; }
        private CharacterBehaviour Cb { get; set; }
        

        private void StartProcessing(float hasteWeight)
        {
            CastingWeight = castingTime * CharacterUtility.GetHasteValue(hasteWeight);
            enabled       = true;
        }

        private void StopProcessing()
        {
            Progress.Value = 0f;
            enabled               = false;
        }

        private void Awake()
        {
            Progress.Value = 0f;
            CastingWeight  = castingTime;
            enabled        = false;

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
            if (Progress.Value < CastingWeight)
            {
                Progress.Value += Time.deltaTime;
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
            
            castingTime = skillData.ProcessTime;
        }
#endif
    }
}