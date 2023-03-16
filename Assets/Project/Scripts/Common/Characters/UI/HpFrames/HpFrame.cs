using System;
using Common.UI;
using UnityEngine;

namespace Common.Characters.UI.HpFrames
{
    public class HpFrame : MonoBehaviour
    {
        [SerializeField] private ImageFiller hpBar;

        private CharacterBehaviour cb;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        private void OnEnable()
        {
            hpBar.Register(Cb.DynamicStatEntry.Hp, cb.StatTable.MaxHp);
        }

        private void OnDisable()
        {
            hpBar.Unregister();
        }
    }
}
