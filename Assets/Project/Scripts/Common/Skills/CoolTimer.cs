using UnityEngine;

namespace Common.Skills
{
    /* SkillComponent CoolTime */
    /* SkillBehaviour GlobalCoolTime */
    public class CoolTimer : MonoBehaviour, IEditable
    {
        [SerializeField] private SectionType startCoolingMoment;
        [SerializeField] private float coolTime;

        public float CoolTime => coolTime;
        public FloatEvent RemainCoolTime { get; } = new();
        

        private void StartCooling()
        {
            enabled              = true;
            RemainCoolTime.Value = coolTime;
        }

        private void StopCooling()
        {
            RemainCoolTime.Value = 0f;
            enabled              = false;
        }
        
        private bool IsCoolTimeReady()
        {
            return RemainCoolTime.Value <= 0.0f;
        }

        private void Awake()
        {
            if (!TryGetComponent(out IOldConditionalSequence sequence))
            {
                Debug.LogError("Require SKillComponent");
                return;
            }

            sequence.Conditions.Add("IsCoolTimeReady", IsCoolTimeReady);
            sequence.ConvertSection(startCoolingMoment).Register("StartCooling", StartCooling);
        }

        private void Update()
        {
            if (RemainCoolTime.Value > 0f)
            {
                RemainCoolTime.Value -= Time.deltaTime;
            }
            else
            {
                StopCooling();
            }
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!TryGetComponent<IDataIndexer>(out var indexer)) return;

            var skillData  = Database.SkillSheetData(indexer.DataIndex);
            coolTime = skillData.CoolTime;
        }
#endif
    }
}
