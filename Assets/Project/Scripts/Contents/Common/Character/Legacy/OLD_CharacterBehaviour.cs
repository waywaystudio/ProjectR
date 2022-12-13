using System;
using System.Collections.Generic;
using Common.Character.Graphic;
using Common.Character.Operation;
using Common.Character.Operation.Combating;
using Core;
using Spine.Unity;
using UnityEngine;

namespace Common.Character
{
    public class OLD_CharacterBehaviour : MonoBehaviour, ISavable, IControlModel
    {
        [SerializeField] string characterName = string.Empty;
        [SerializeField] private int characterID;
        [SerializeField] private string characterClass;
        [SerializeField] private List<string> equipmentList;
        [SerializeField] private List<string> extraList;

        public void Initialize(string characterName)
        {
            // Field
            // name
            // ID
            // class
            // equipment
            // spineImage (race + equipment maybe...)
            // extraStatus;
        }

        // Temp for Test
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackSpeed = 1f;
        [SerializeField] private float range;
        private ICombatAttribution temp;
        public float Range => range;

        // Data
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        
        // Operation
        [SerializeField] private Combat combat;
        [SerializeField] private PlayerController controller;
        [SerializeField] private Operation.Pathfinding pathfinding;
        [SerializeField] private Searching searching;
        
        // Graphic
        [SerializeField] private AnimationModel animationModel;
        [SerializeField] private AnimationEventModel animationEvent;

        // ICombatTaker
        public GameObject TargetObject => gameObject;

        // Behavior
        public bool HasPath => pathfinding.HasPath;
        public bool IsDestinationReached => !HasPath || pathfinding.IsReached;
        // public bool IsInRange
        // {
        //     get
        //     {
        //         if (FocusTarget.IsNullOrEmpty()) return false;
        //
        //         return Vector3.Distance(transform.position, FocusTarget.transform.position) <= range;
        //     }
        // }

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
        public Vector3 Destination => Vector3.zero;
        public Searching Searching => searching;
        // public GameObject FocusTarget => Searching.FocusTarget;

        public void Idle()
        {
            animationModel.Idle();
        }

        public void Attack(GameObject target)
        {
            // animationModel.Attack(false, Idle);
        }

        public void CombatAction()
        {
            
            // combat.Invoke(temp);
            // animationModel.Play(combat.AnimationKey, 0, false, Idle);
        }

        public void UseItem(GameObject item)
        {
            
        }

        public void Interaction(GameObject target)
        {
            
        }

        public void Walk(Vector3 destination)
        {
            // pathfinding.OLD_Move(destination, moveSpeed);
            // animationModel.Walk();
        }

        public void Run(Vector3 destination)
        {
            // pathfinding.OLD_Move(destination, moveSpeed);
            // animationModel.Run();
        }

        public void Stop()
        {
            pathfinding.Stop();
            animationModel.Idle();
        }

        public void Death()
        {
            
        }

        private void Awake()
        {
            skeletonAnimation ??= GetComponentInChildren<SkeletonAnimation>();
            controller ??= GetComponentInChildren<PlayerController>();
            combat ??= GetComponentInChildren<Combat>();
            pathfinding ??= GetComponentInChildren<Operation.Pathfinding>();
            searching ??= GetComponentInChildren<Searching>();
            animationModel ??= GetComponentInChildren<AnimationModel>();
            animationEvent ??= GetComponentInChildren<AnimationEventModel>();
            
            controller.Initialize(GetComponent<Rigidbody>());
            // searching.Initialize(range);
        }

        private void Update()
        {
            // searching.UpdateTargeting();
            animationModel.Flip(transform.forward);
        }

        public void Save()
        {
        }

        public void Load()
        {
        }

        public void UpdateState()
        {
            controller.DirectionControl();
        }
    }
}
