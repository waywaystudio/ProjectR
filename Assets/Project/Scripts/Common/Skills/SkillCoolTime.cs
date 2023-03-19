using UnityEngine;

namespace Common.Skills
{
    public class SkillCoolTime : MonoBehaviour, IEditable
    {
        [SerializeField] private SectionType startCoolingMoment;
        [SerializeField] private float coolTime;

        public float CoolTime => coolTime;
        public FloatEvent RemainCoolTime { get; } = new(0f, float.MaxValue);
        

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
            if (!TryGetComponent(out SkillComponent skill))
            {
                Debug.LogError("Require SKillComponent");
                return;
            }

            skill.Conditions.Register("_IsCoolTimeReady", IsCoolTimeReady);
            skill.ConvertSection(startCoolingMoment).Register("_StartCooling", StartCooling);
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
            var actionCode = GetComponent<IDataIndexer>().ActionCode;
            var skillData  = Database.SkillSheetData(actionCode);
            
            coolTime = skillData.CoolTime;
        }
#endif
    }
}
