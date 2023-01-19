using System;
using Character;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.MonsterFrames
{
    public class BossFrame : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI bossNameText;

        private int instanceID;
        private MonsterBehaviour mb;

        public void Initialize(MonsterBehaviour mb)
        {
            this.mb           = mb;
            bossNameText.text = mb.Name;
            
            mb.DynamicStatEntry.Hp.Register(instanceID, FillHealthBar);

            FillHealthBar(mb.DynamicStatEntry.Hp.Value);
        }
        
        
        private void FillHealthBar(float hp)
        {
            var normalHp = hp / mb.StatTable.MaxHp;

            healthBar.DOFillAmount(normalHp, 0.1f);
            healthText.text = hp.ToString("N0");
        }

        private void Awake()
        {
            instanceID = GetInstanceID();
        }
        
        private void OnDisable()
        {
            if (mb.IsNullOrEmpty()) return;
            
            mb.DynamicStatEntry.Hp.Unregister(instanceID);
        }
    }
}
