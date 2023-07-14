using Singleton;
using UnityEngine;

namespace Common.UI
{
    public class MainUI : MonoSingleton<MainUI>
    {
        [SerializeField] private FadePanel fadePanel;

        public static FadePanel FadePanel => Instance.fadePanel;
    }
}