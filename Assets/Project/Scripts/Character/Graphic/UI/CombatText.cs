using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Common.Character.Graphic.UI
{
    public class CombatText : MonoBehaviour
    {
        [SerializeField] private float upOffset = 1.5f;
        [SerializeField] private float duration = 1.0f;
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private List<Transform> combatTextList;

        private TextMeshProUGUI tmpText;

        public void DrawCombatText(CombatLog log)
        {
            if (!TryGetTextComponent(out var textComponent)) return;
            if (!textComponent.TryGetComponent(out tmpText)) return;

            var outputText = !log.IsHit 
                ? "Dodge!" 
                : log.Value.ToString("N0", CultureInfo.InvariantCulture);

            tmpText.text = outputText;

            textComponent.gameObject.SetActive(true);
            textComponent.DOLocalMoveY(upOffset, duration).OnComplete(() => ReturnToPool(textComponent));
        
            if (log.IsCritical) tmpText.DOColor(Color.red, 0.3f);
        }
    
        private bool TryGetTextComponent(out Transform textComponent)
        {
            textComponent = combatTextList.Find(x => x.gameObject.activeSelf == false);

            return textComponent != null;
        }

        private void ReturnToPool(Transform textComponent)
        {
            textComponent.gameObject.SetActive(false);
            textComponent.DOLocalMoveY(0f, 0f);
            tmpText.DOColor(Color.white, 0f);
        }

        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            if (combatTextList.IsNullOrEmpty())
            {
                combatTextList = GetComponentsInChildren<Transform>(true).ToList();
            }
        }

        private void OnEnable() => cb.OnCombatPassive.Register(GetInstanceID(), DrawCombatText);
        private void OnDisable() => cb.OnCombatPassive.Unregister(GetInstanceID());

#if UNITY_EDITOR
        private void Reset() => GetTxtList();
        
        [Button]
        private void GetTxtList()
        {
            combatTextList = GetComponentsInChildren<Transform>(true).ToList();
        }
#endif
    }
}
