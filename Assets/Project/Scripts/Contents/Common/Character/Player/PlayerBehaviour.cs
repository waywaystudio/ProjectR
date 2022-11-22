using Core;
using Main;
using Spine.Unity;
using UnityEngine;

namespace Common.Character.Player
{
    public class PlayerBehaviour : MonoBehaviour, ISavable, IControlModel
    {
        public float MoveSpeed = 5f;
        public CharacterState State;

        // Data
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        
        // Operation
        [SerializeField] private PlayerController controller;
        [SerializeField] private CharacterPathfinding characterPathfinding;
        
        // Graphic
        [SerializeField] private CharacterAnimationModel animationModel;
        [SerializeField] private CharacterAnimationEventModel animationEvent;
        

        public void Idle()
        {
            animationModel.Idle();
        }

        public void Attack(GameObject target)
        {
            // OnAttack?.Invoke(target);
        }

        public void Defence()
        {
            
        }

        public void Damaged(GameObject from)
        {
            
        }

        public void Skill(GameObject target)
        {
            
        }

        public void UseItem(GameObject item)
        {
            
        }

        public void Interaction(GameObject target)
        {
            
        }

        public void Walk(Vector3 destination)
        {
            characterPathfinding.Move(destination, MoveSpeed);
            animationModel.Walk();
        }

        public void Run(Vector3 position)
        {
            
        }

        public void Death()
        {
            
        }

        private void Awake()
        {
            skeletonAnimation ??= GetComponentInChildren<SkeletonAnimation>();
            controller ??= GetComponentInChildren<PlayerController>();
            characterPathfinding ??= GetComponentInChildren<CharacterPathfinding>();
            animationModel ??= GetComponentInChildren<CharacterAnimationModel>();
            animationEvent ??= GetComponentInChildren<CharacterAnimationEventModel>();
            
            controller.Initialize(GetComponent<Rigidbody>());
            characterPathfinding.Initialize(Idle);
            animationModel.Initialize(skeletonAnimation);
        }

        private void Start()
        {
            // Temp
            MainGame.InputManager.Register(this);
        }

        private void Update()
        {
            animationModel.Flip(transform.forward);

            #region TEST
            if (!Input.GetMouseButtonDown(0)) return;
        
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            // ReSharper disable once PossibleNullReferenceException
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (!Physics.Raycast(ray, out var hit)) return;

            Walk(hit.point);

            #endregion
        }

        public void Save()
        {
            // 씬에 따라서 저장해야 하는 정보가 다름 특히 위치
            // 세이브가 가능한 Town씬에서는 위치를 저장하는게 의미가 있음.
            // SaveManager.Save("playerTransform.position", transform.position);
            // SaveManager.Save("playerTransform.rotation", transform.rotation);
        }

        public void Load()
        {
            // 마찬가지로 씬에 따라서 불러와야 하는 정보가 다를 수 있음.
            // 레이드 씬에서는 플레이어 위치정보를 불러올 필요가 없음.
            // var position = SaveManager.Load("playerTransform.position", Vector3.zero);
            // var rotation = SaveManager.Load("playerTransform.rotation", Quaternion.identity);
            // transform.SetPositionAndRotation(position, rotation);
        }

        public void UpdateState()
        {
            controller.DirectionControl();
        }
    }
}
