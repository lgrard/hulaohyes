using System.Collections;
using UnityEngine;
using hulaohyes.Assets.Scripts.Targettable.Pickable.Player;
using hulaohyes.Assets.Scripts.Components.Enemy;

namespace hulaohyes.Assets.Scripts.Targettable.Pickable.Enemy
{
    public class EnemyController2D : MonoBehaviour, IPickable, ITargettable
    {
        public delegate void EventHandler();
        public EventHandler onGetThrown;
        public EventHandler onGetDropped;
        public EventHandler onGetPickedUp;
        public EventHandler onTargetLoss;

        public delegate void TypedEventHandler<T>(T pValue);
        public TypedEventHandler<Transform> onTargetAquire;

        private Rigidbody2D _rb;
        private bool isPickable = true;

        [SerializeField] private EnemyDatas _enemyDataSet = null;

        private void Awake()
        {
            foreach (EnemyComponent lComponent in GetComponentsInChildren<EnemyComponent>())
                lComponent.enemy = this;
        }

        private void Start() => Init();

        private void Init()
        {
            _rb = GetComponent<Rigidbody2D>();
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
            onGetPickedUp?.Invoke();
        }


        //Getters
        public bool isTargettable => isPickable;
        public Transform Transform => transform;
        public EnemyDatas enemyDataSet => _enemyDataSet;
        public Rigidbody2D rb => _rb;
    }
}