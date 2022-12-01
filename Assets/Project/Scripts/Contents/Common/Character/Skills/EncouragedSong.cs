using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Skills
{
    public class EncouragedSong : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private string skillName;
        [SerializeField] private float baseCoolTime;
        [SerializeField] private float castingTime;
        [SerializeField] private float range;
        [SerializeField] private int priority;
        [SerializeField] private List<string> assignedClass = new();
        [SerializeField] private string motionType;
        [SerializeField] private int targetCount;
        [SerializeField] private string targetLayer;
        [SerializeField] private string skillType;
        
        private float remainCoolTime;

        public bool IsCoolOn => RemainCoolTime <= 0.0f;
        public string SkillName => skillName;
        public float CoolTime => baseCoolTime;
        public float RemainCoolTime
        {
            get => remainCoolTime;
            set => remainCoolTime = Mathf.Max(0, value);
        }
        

        public void DoSkill(GameObject target)
        {
            if (!PreRequisition())
            {
                Debug.Log("Not Ready");
                return;
            }
            
            // Implement Combat.
            // var ally = target.GetComponent<IHealable>();
            // 이 때 ally는 대략 바라보는 타겟 정도의 느낌이고, 실제 치유가 들어가는 대상은 다른 메카닉.
            // var allies = GetTargets();
            // allies.Foreach(x => x.GetHeal((IHealable) healValue);

            remainCoolTime = CoolTime;
        }

        public void DecreaseCoolTime() => DecreaseCoolTime(Time.deltaTime);
        public void DecreaseCoolTime(float tick) => RemainCoolTime -= tick;

        private bool PreRequisition()
        {
            var result =
                IsCoolOn;
            
            return result;
        }
        
        
        private float remainCastingTime;
        private Coroutine castingCoroutine;
        public float RemainCastingTime
        {
            get => remainCastingTime;
            set => remainCastingTime = Mathf.Max(0, value);
        }
        
        private void Awake()
        {
            remainCastingTime = castingTime;
        }
        
        private IEnumerator CastSkill(float tick)
        {
            while (RemainCastingTime > 0)
            {
                RemainCastingTime -= tick;
                yield return null;
            }
            
            yield return true;
        }

        private IEnumerator DoSkill(float tick)
        {
            yield return null;
        }
        
#if UNITY_EDITOR
        #region EditorOnly
        
        [OnInspectorInit]
        private void Initialize() => Initialize("Encouraged Song");
        private void Initialize(string skillName)
        {
            Finder.TryGetObject(out Data.ContentData.SkillData skillData);

            var staticData = skillData.SkillList.Find(x => x.SkillName.Equals(skillName.ToPascalCase()));
            if (staticData is null)
            {
                Debug.LogWarning($"Can't Find {skillName} in {skillData} ScriptableObject");
                return;
            }

            id = staticData.ID;
            this.skillName = staticData.SkillName;
            baseCoolTime = staticData.BaseCoolTime;
            castingTime = staticData.CastingTime;
            range = staticData.Range;
            priority = staticData.Priority;
            assignedClass = staticData.AssignedClass.ToList();
            motionType = staticData.MotionType;
            targetCount = staticData.TargetCount;
            targetLayer = staticData.TargetLayer;
            skillType = staticData.SkillType;
        }
        
        #endregion
#endif
    }
}
