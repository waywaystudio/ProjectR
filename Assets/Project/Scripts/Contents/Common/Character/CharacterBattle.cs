using System;
using UnityEngine;

namespace Common.Character
{
    public class CharacterBattle : MonoBehaviour
    {
        // SkillDataList;
        // each coolTime tick;
        // Update
        
        [SerializeField] private float coolTime = 2f;

        private bool isReady;
        private float tick;
        private float remainCoolTime;

        public bool IsReady => isReady;

        public void DoSkill(GameObject target)
        {
            if (!isReady)
            {
                Debug.Log("Not Ready");
                return;
            }

            isReady = false;
            remainCoolTime = coolTime;
        }

        private void Awake()
        {
            tick = Time.deltaTime;
        }

        private void Update()
        {
            if (remainCoolTime > 0.0f)
            {
                remainCoolTime -= tick;
                return;
            }

            isReady = true;
        }
    }
}
