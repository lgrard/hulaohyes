using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using hulaohyes.player.states;
using hulaohyes.inputs;
using hulaohyes.camera;
using UnityEngine.Animations;

namespace hulaohyes.player
{
    public class PlayerController : Pickable
    {
        const float KNOCK_BACK_AMOUNT = 1.5f;
        const float KNOCK_BACK_TIME = 0.1f;

        private GameManager gameManager;
        private Animator playerAnimator;
        private PlayerStateMachine stateMachine;
        private PlayerInput playerInput;
        private ControlScheme controlScheme;
        private Transform cameraContainer;
        private SkinnedMeshRenderer renderer;

        public int playerIndex = 0;

        [Header("HP values")]
        [SerializeField] int MAX_HP = 3;                                                                                                         //const to change
        private int hp;

        [Header("Objects and components")]
        [Tooltip("The transform where pick up target is stored")]
        public Transform pickUpPoint;

        [Header("Particles list and effects")]
        [SerializeField] List<ParticleSystem> playerParticles;
        [SerializeField] public LookAtConstraint pickIndicator;

        [Header("Player values")]
        public float MOVEMENT_SPEED = 6;                                                                                                //const to change
        public float JUMP_HEIGHT = 8;                                                                                                   //const to change
        public float GROUND_CHECK_DISTANCE = 0.5f;
        public float PICK_UP_DISTANCE =2f;                                                                                              //const to change
        public float VERTICAL_PROPEL_HEIGHT = 8f;                                                                                       //const to change

        [HideInInspector]
        public Pickable pickUpTarget;

        [Header("Debug")]
        [SerializeField] string currentState;

        ///Standard and physic GameLoops
        private void Update() => stateMachine.CurrentState.LoopLogic();
        private void FixedUpdate()
        {
            currentState = stateMachine.CurrentState.ToString();
            stateMachine.CurrentState.PhysLoopLogic();
            rb.AddForce(Physics.gravity * _gravity, ForceMode.Acceleration);
        }

        /// The player takes a certain amount of damage
        /// <param name="pDamage"> Amount of damage taken </param>
        public void TakeDamage(int pDamage, Transform pDealer)
        {
            rb.velocity = Vector3.zero;
            hp -= pDamage;
            if (hp >= 0 && stateMachine.CurrentState == stateMachine.Carrying)
                stateMachine.Carrying.TakeCarryDamage();

            else if (hp > 0 && stateMachine.CurrentState != stateMachine.Carrying && stateMachine.CurrentState != stateMachine.Carried)
            {
                StartCoroutine(KnockBack(pDealer));
                playerAnimator.SetTrigger("TakeDamage");
                stateMachine.CurrentState = stateMachine.Wait;
            }

            else if (hp <= 0)
            {
                StartCoroutine(KnockBack(pDealer));
                playerAnimator.SetTrigger("Dies");
                stateMachine.CurrentState = stateMachine.Downed;
            }
        }

        private IEnumerator KnockBack(Transform pOrigin)
        {
            float lTimeStamp = KNOCK_BACK_TIME;
            Vector3 lFirstPosition = transform.position;
            Vector3 lKnockBackDestination = transform.position + (pOrigin.forward * KNOCK_BACK_AMOUNT);
            transform.rotation = Quaternion.LookRotation(pOrigin.position- transform.position, Vector3.up);
            while (lTimeStamp > 0)
            {
                transform.position = Vector3.Lerp(lKnockBackDestination, lFirstPosition, lTimeStamp / KNOCK_BACK_TIME);
                lTimeStamp -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public void SetPlayerDevice()
        {
            InputDevice lDevice = DeviceManager.GetInputDevice(playerIndex);
            if (lDevice != null) playerInput.SwitchCurrentControlScheme(lDevice);
        }
        public void DropTarget()
        {
            rb.velocity = rb.velocity / 1.2f;
            stateMachine.CurrentState = stateMachine.Wait;
        }
        override protected void Init()
        {
            base.Init();
            isPickableState = false;
            cameraContainer = CameraManager.getInstance().GetCamera(playerIndex).transform;
            playerAnimator = GetComponent<Animator>();
            controlScheme = new ControlScheme();
            playerInput = GetComponent<PlayerInput>();
            renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            playerInput.actions = controlScheme.asset;
            SetPlayerDevice();
            stateMachine = new PlayerStateMachine(this, controlScheme, cameraContainer, rb, playerAnimator, playerParticles, _collider);
            ConstraintSource lCamLookAt = new ConstraintSource();
            lCamLookAt.sourceTransform = cameraContainer;
            lCamLookAt.weight = 1;
            pickIndicator.SetSource(0,lCamLookAt);
            pickIndicator.constraintActive = true;

            hp = MAX_HP;
        }

        public void Spawn()
        {
            hp = MAX_HP;

            renderer.enabled = true;
            base._collider.enabled = true;
            base.rb.isKinematic = false;
        }

        public void Die()
        {
            renderer.enabled = false;
            base._collider.enabled = false;
            base.rb.isKinematic = true;
        }

        override public void Propel()
        {
            stateMachine.CurrentState = stateMachine.Thrown;
            base.Propel();
        }

        override public void Drop()
        {
            stateMachine.CurrentState = stateMachine.Thrown;
            base.Drop();
        }

        override public void GetPicked(PlayerController pPicker)
        {
            base.GetPicked(pPicker);
            if (pickUpTarget != null) DropTarget();
            stateMachine.CurrentState = stateMachine.Carried;
        }
        protected override void HitElseThrown(Collider pCollider)
        {
            base.HitElseThrown(pCollider);
            stateMachine.CurrentState = stateMachine.Running;
        }
        protected override void HitElseDropped(Collider pCollider)
        {
            base.HitElseDropped(pCollider);
            stateMachine.CurrentState = stateMachine.Running;
        }

        bool isThrown => stateMachine.CurrentState == stateMachine.Thrown;
        public Animator Animator => playerAnimator;
        public ControlScheme getActiveControlScheme() { return controlScheme; }

        protected override void OnGizmos()
        {
            Gizmos.DrawLine(transform.position + new Vector3(0,0.5f,0), (transform.forward * PICK_UP_DISTANCE+transform.position+new Vector3(0,0.5f,0)));             //GroundCheck debug line
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GROUND_CHECK_DISTANCE, transform.position.z));             //GroundCheck debug line
            base.OnGizmos();
        }

        ///Destroy player's object and delete references
        public void destroyPlayer()
        {
            Destroy(gameObject);
        }
    }
}