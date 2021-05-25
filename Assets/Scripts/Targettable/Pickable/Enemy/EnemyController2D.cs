using System.Collections;
using UnityEngine;
using hulaohyes.Assets.Scripts.Targettable.Pickable.Player;
using hulaohyes.Assets.Scripts.Components.Enemy;
using hulaohyes.Assets.Scripts.Components.Enemy.Walker;

namespace hulaohyes.Assets.Scripts.Targettable.Pickable.Enemy
{
    public class EnemyController2D : MonoBehaviour, IPickable, ITargettable
    {
        public delegate void EventHandler();
        public EventHandler onGetThrown;
        public EventHandler onGetDropped;
        public EventHandler onGetPickedUp;
        public EventHandler onActive;
        public EventHandler onRecover;
        public EventHandler onEndAttack;
        public EventHandler onChangeDirection;

        public delegate void TypedEventHandler<T>(T pValue);
        public TypedEventHandler<Transform> onTargetAquire;

        private Rigidbody2D _rb;
        private bool isPickable = true;
        private int _direction = 1;

        [SerializeField] private EnemyDatas _enemyDataSet = null;
        [SerializeField] private EnemyTargetting targetting = null;
        [SerializeField] private WalkerRush attacking = null;

        private void Awake()
        {
            foreach (EnemyComponent lComponent in GetComponentsInChildren<EnemyComponent>())
                lComponent.enemy = this;
        }

        private void Start() => Init();

        private void Init()
        {
            _rb = GetComponent<Rigidbody2D>();
            onTargetAquire += OnTargetAquire;
            onRecover += OnRecover;
            onEndAttack += OnEndAttack;
        }

        public void GetThrown(Vector2 pVelocity)
        {
            transform.parent = null;
            _rb.isKinematic = false;
            _rb.velocity = pVelocity;
            isPickable = true;
            onGetThrown?.Invoke();
        }
        
        public void GetDropped(Vector2 pVelocity)
        {
            transform.parent = null;
            _rb.isKinematic = false;
            _rb.velocity = pVelocity;
            isPickable = true;
            onGetDropped?.Invoke();
        }

        public void GetPickedUp(PlayerController2D pPicker)
        {
            _rb.isKinematic = true;
            transform.eulerAngles = Vector3.zero;
            isPickable = false;
            targetting.enabled = false;
            attacking.enabled = false;
            onGetPickedUp?.Invoke();
        }

        //State component logic
        private void ClearState()
        {
            _rb.isKinematic = true;
            isPickable = true;
        }

        private void OnTargetAquire(Transform pTarget)
        {
            targetting.enabled = false;
            isPickable = false;
        }

        private void OnRecover()
        {
            isPickable = true;
        }

        private void OnEndAttack()
        {
            ClearState();
            targetting.enabled = true;
        }


        public int direction
        {
            get => _direction;
            set => _direction = Mathf.Clamp(value, -1, 1);
        }

        //Getters
        public bool isTargettable => isPickable;
        public Transform Transform => transform;
        public EnemyDatas enemyDataSet => _enemyDataSet;
        public Rigidbody2D rb => _rb;
    }
}