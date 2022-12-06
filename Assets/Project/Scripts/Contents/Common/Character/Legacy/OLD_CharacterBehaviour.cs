using System;
using System.Collections.Generic;
using Common.Character.Skills.Entity;
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
        [SerializeField] private CharacterBattle combat;
        [SerializeField] private PlayerController controller;
        [SerializeField] private CharacterPathfinding pathfinding;
        [SerializeField] private CharacterTargeting targeting;
        
        // Graphic
        [SerializeField] private CharacterAnimationModel animationModel;
        [SerializeField] private CharacterAnimationEventModel animationEvent;

        // ICombatTaker
        public GameObject TargetObject => gameObject;

        // Behavior
        public bool HasPath => pathfinding.HasPath;
        public bool IsDestinationReached => !HasPath || pathfinding.IsReached;
        public bool IsInRange
        {
            get
            {
                if (FocusTarget.IsNullOrEmpty()) return false;

                return Vector3.Distance(transform.position, FocusTarget.transform.position) <= range;
            }
        }

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
        public Vector3 Destination => Vector3.zero;
        public CharacterTargeting Targeting => targeting;
        public GameObject FocusTarget => Targeting.FocusTarget;

        public void Idle()
        {
            animationModel.Idle();
        }

        public void Attack(GameObject target)
        {
            animationModel.Attack(false, Idle);
        }

        public void CombatAction()
        {
            combat.Invoke(temp);
            animationModel.Play(combat.AnimationKey, 0, false, Idle);
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
            animationModel.Walk();
        }

        public void Run(Vector3 destination)
        {
            // pathfinding.OLD_Move(destination, moveSpeed);
            animationModel.Run();
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
            combat ??= GetComponentInChildren<CharacterBattle>();
            pathfinding ??= GetComponentInChildren<CharacterPathfinding>();
            targeting ??= GetComponentInChildren<CharacterTargeting>();
            animationModel ??= GetComponentInChildren<CharacterAnimationModel>();
            animationEvent ??= GetComponentInChildren<CharacterAnimationEventModel>();
            
            controller.Initialize(GetComponent<Rigidbody>());
            targeting.Initialize(range);
        }

        private void Update()
        {
            targeting.UpdateTargeting();
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
