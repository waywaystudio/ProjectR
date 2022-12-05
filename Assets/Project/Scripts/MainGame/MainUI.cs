using Core.Singleton;
using UnityEngine;

namespace MainGame
{
    using UI;
    
    public class MainUI : MonoSingleton<MainUI>
    {
        [SerializeField] private FadePanel fadePanel;

        public static FadePanel FadePanel => Instance.fadePanel;
    }
}
