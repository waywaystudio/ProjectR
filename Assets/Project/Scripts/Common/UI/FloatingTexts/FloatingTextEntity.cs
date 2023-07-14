using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.UI.FloatingTexts
{
    [Serializable]
    public class FloatingTextEntity
    {
        [EnumPaging]
        public CombatEntityType CombatEntityType;
        
        [ColorPalette("Fall")]
        public Color TextColor;
        
        [PropertyRange(1f, 5f)]
        public float ImpactScale = 2.0f;
        
        [PropertyRange(0.1f, 1f)]
        public float ImpactDuration = 0.45f;
        
        [PropertyRange(0.1f, 2f)]
        public float PivotSpreadRange = 0.5f;
        
        [PropertyRange(3f, 50f)]
        public float MoveUpDistance = 25f;
        
        [PropertyRange(0.1f, 1f)]
        public float MoveUpDuration = 0.35f;

        public Ease MoveUpEase;
        
        [PropertyRange(0f, 1f)]
        public float FadeDuration = 0.35f;
    }
}
