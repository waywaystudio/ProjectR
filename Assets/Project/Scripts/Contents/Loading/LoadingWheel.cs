using Main;
using UnityEngine;

namespace Loading
{
    public class LoadingWheel : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private RectTransform barTransform;

        private float barMaxLength = 800.0f;
        private float barHeight = 10.0f;
        private static readonly int RotateWheel = Animator.StringToHash("RotateWheel");

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            Play();
        }

        private void Update()
        {
            var progress = MainGame.SceneManager.Progress
                           * barMaxLength;

            barTransform.sizeDelta = new Vector2(progress, barHeight);
        }

        public void Play()
        {
            animator.SetBool(RotateWheel, true);
        }

        public void Stop()
        {
            animator.SetBool(RotateWheel, false);
        }
    }
}
