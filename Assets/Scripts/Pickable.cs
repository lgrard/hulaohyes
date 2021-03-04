using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;
using hulaohyes.enemy;

public class Pickable : MonoBehaviour
{
    //private const float THROW_FORCE = 20f;
    //private const float DROP_FORCE = 9f;
    //private const float THROW_ANGLE_OFFSET = 0.15f;
    //private const float DROP_ANGLE_OFFSET = 0.15f;
    //private const float GRAVITY_AMOUNT_RISE = 2f;
    //private const float GRAVITY_AMOUNT_FALL = 4f;


    private LayerMask _collisionLayer;
    private Vector3 _forwardVelocity;

    protected private PlayerController _currentPicker = null;
    protected private Rigidbody _rb;
    protected private Collider _collider;

    [Range(1, 30)] public float THROW_FORCE = 20f;                                                                                                  //const to change
    [Range(1, 30)] public float DROP_FORCE = 9f;                                                                                                    //const to change
    [Range(0,1)]public float THROW_ANGLE_OFFSET = 0.15f;                                                                                            //const to change
    [Range(0, 1)]public float DROP_ANGLE_OFFSET = 0.15f;                                                                                            //const to change
    [Range(0, 10)] public float GRAVITY_AMOUNT_RISE = 2f;                                                                                           //const to change
    [Range(0, 10)] public float GRAVITY_AMOUNT_FALL = 4f;                                                                                           //const to change
                                      

    [SerializeField] bool _drawGizmos = true;

    public bool isPickable => _currentPicker == null;
    protected float _gravity => (_rb.velocity.y < 0) ? GRAVITY_AMOUNT_FALL : GRAVITY_AMOUNT_RISE;


    private void Start() => Init();

    protected virtual void Init()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _collisionLayer = LayerMask.GetMask("Enemy","Player","KillZone","Default","Bricks","Ground");
    }

    public virtual void Propel() => GetDropped(THROW_FORCE, THROW_ANGLE_OFFSET);
    public virtual void Drop() => GetDropped(DROP_FORCE, DROP_ANGLE_OFFSET);

    public virtual void GetPicked(PlayerController pPicker)
    {
        transform.parent = pPicker.pickUpPoint;
        _currentPicker = pPicker;
        _rb.isKinematic = true;

        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        _collider.isTrigger = true;
    }

    private void GetDropped(float pDropForce, float pAngleOffset)
    {
        Vector3 lDiretionOffset = new Vector3(0, -pAngleOffset);
        _currentPicker.DropTarget();
        transform.parent = null;
        _currentPicker = null;
        _rb.isKinematic = false;
        _forwardVelocity = (transform.forward - lDiretionOffset) * pDropForce;
        _rb.velocity = _forwardVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_collisionLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            HitSomething();
            if (gameObject.TryGetComponent<EnemyController>(out EnemyController pEnemy)) HitEnemy(pEnemy);
        }

        else Debug.Log(gameObject.name + ": " +other.gameObject.layer);
    }

    protected virtual void HitEnemy(EnemyController pEnemy)
    {
        pEnemy.TakeDamage(1);
        pEnemy.StartCoroutine(pEnemy.KnockBack(this.transform));
    }

    protected virtual void HitSomething() => _collider.isTrigger = false;

    protected virtual void OnGizmos()
    {
        if(_currentPicker != null)
        {
            Gizmos.color = Color.red;
            Vector3 lDiretionOffsetThrow = new Vector3(0, -THROW_ANGLE_OFFSET);
            Gizmos.DrawLine(transform.position, transform.position+(transform.forward - lDiretionOffsetThrow) * 5);
            Gizmos.DrawSphere(transform.position + (transform.forward - lDiretionOffsetThrow) * 5, 0.1f);

            Gizmos.color = Color.magenta;
            Vector3 lDiretionOffsetDrop = new Vector3(0, -DROP_ANGLE_OFFSET);
            Gizmos.DrawLine(transform.position, transform.position+(transform.forward - lDiretionOffsetDrop) * 3);
            Gizmos.DrawSphere(transform.position + (transform.forward - lDiretionOffsetDrop) * 3, 0.1f);

            Gizmos.color = Color.white;
        }
    }

    private void OnDrawGizmos() { if (_drawGizmos) OnGizmos(); }
}
