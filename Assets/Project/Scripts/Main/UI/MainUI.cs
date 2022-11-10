using UnityEngine;

namespace Main.UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private FadePanel fadePanel;

        public FadePanel FadePanel => fadePanel;
    }
}
