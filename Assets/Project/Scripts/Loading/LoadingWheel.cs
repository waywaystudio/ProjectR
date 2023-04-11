using UnityEngine;

namespace Loading
{
    public class LoadingWheel : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private RectTransform barTransform;

        private readonly int rotateWheel = Animator.StringToHash("RotateWheel");

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            Play();
        }

        public void Play()
        {
            animator.SetBool(rotateWheel, true);
        }

        public void Stop()
        {
            animator.SetBool(rotateWheel, false);
        }
    }
}
