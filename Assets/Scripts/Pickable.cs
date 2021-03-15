using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;
using hulaohyes.enemy;

public class Pickable : MonoBehaviour
{
    private const float DEG2RAD = Mathf.PI / 180;
    //private const float THROW_FORCE = 20f;
    //private const float DROP_FORCE = 9f;
    //private const float THROW_ANGLE_OFFSET = 0.15f;
    //private const float DROP_ANGLE_OFFSET = 0.15f;
    //private const float GRAVITY_AMOUNT_RISE = 2f;
    //private const float GRAVITY_AMOUNT_FALL = 4f;

    protected LayerMask killZoneLayer;
    private LayerMask collisionLayer;
    private Vector3 forwardVelocity;
    private TrailRenderer speedTrail;

    protected private PlayerController _currentPicker = null;
    protected private Rigidbody rb;
    protected private Collider _collider;
    protected bool isDropped = false;
    protected bool isThrown = false;
    protected bool _isPicked = false;

    [Range(1, 30)] public float THROW_FORCE = 20f;                                                                                                  //const to change
    [Range(1, 30)] public float DROP_FORCE = 9f;                                                                                                    //const to change
    [Range(0, 90)] public float THROW_ANGLE_OFFSET = 10f;                                                                                            //const to change
    [Range(0, 90)] public float DROP_ANGLE_OFFSET = 10f;                                                                                            //const to change
    [Range(0, 10)] public float GRAVITY_AMOUNT_RISE = 2f;                                                                                           //const to change
    [Range(0, 10)] public float GRAVITY_AMOUNT_FALL = 4f;                                                                                           //const to change
    [SerializeField] Vector3 _proprelZoneCheck;                                  
    [SerializeField] protected ParticleSystem _impactParticles;

    [SerializeField] bool _drawGizmos = true;

    
    public bool isPickableState = true;

    public bool isPickable =>_currentPicker == null && isPickableState;

    protected float _gravity => (rb.velocity.y < 0) ? GRAVITY_AMOUNT_FALL : GRAVITY_AMOUNT_RISE;


    private void Start() => Init();

    protected virtual void Init()
    {
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        collisionLayer = LayerMask.GetMask("Enemy","Player","Default","Bricks","Ground");
        killZoneLayer = LayerMask.NameToLayer("KillZone");
        speedTrail = GetComponentInChildren<TrailRenderer>();
        if (speedTrail != null) speedTrail.emitting = false;
    }

    public virtual void Propel()
    {
        isDropped = false;
        isThrown = true;
        if (speedTrail != null) speedTrail.emitting = true;
        Vector3 lPropelPos = transform.position + (transform.forward * _proprelZoneCheck.z) - new Vector3(0, 0.5f, 0); ;
        Transform lTarget = Utility.GetClosestTarget(transform, Physics.OverlapBox(lPropelPos, _proprelZoneCheck, transform.rotation, LayerMask.GetMask("Enemy")));
        GetDropped(THROW_FORCE, THROW_ANGLE_OFFSET, lTarget);
    }
    public virtual void Drop()
    {
        isDropped = true;
        isThrown = false;
        GetDropped(DROP_FORCE, DROP_ANGLE_OFFSET);
    }

    public virtual void GetPicked(PlayerController pPicker)
    {
        _isPicked = true;
        transform.parent = pPicker.pickUpPoint;
        rb.isKinematic = true;

        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;

        _currentPicker = pPicker;
    }

    private void GetDropped(float pDropForce, float pAngleOffset, Transform pTarget = null)
    {
        Vector3 lDiretionOffset = new Vector3(0, Mathf.Sin(pAngleOffset * DEG2RAD), 0);
        Vector3 lForwardDirection = transform.forward;

        if (pTarget != null && pTarget.gameObject != gameObject)
            lForwardDirection = ((pTarget.position+ new Vector3(0,1f,0)) - transform.position).normalized;

        _isPicked = false;

        if(_currentPicker!=null) _currentPicker.DropTarget();
        transform.parent = null;
        rb.isKinematic = false;
        forwardVelocity = (lForwardDirection + lDiretionOffset) * pDropForce;
        rb.velocity = forwardVelocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == killZoneLayer)
        {
            Drowns();
            return;
        }

        if (!other.collider.isTrigger)
        {
            if(_currentPicker != null && other.gameObject != _currentPicker.gameObject && !_isPicked)
            {
                _currentPicker = null;
                rb.velocity = Vector3.zero;

                if (isDropped) HitElseDropped(other.collider);

                else if(isThrown)
                {
                    HitElseThrown(other.collider);

                    if (other.gameObject.TryGetComponent<EnemyController>(out EnemyController pEnemy)
                    && gameObject != pEnemy.gameObject) HitEnemy(pEnemy);
                }
            }

            else HitSomething(other);
        }
    }


    private void HitEnemy(EnemyController pEnemy)
    {
        if (_impactParticles != null) _impactParticles.Play();
        pEnemy.destroyEnemy();
    }
    protected virtual void HitSomething(Collision pCollider)
    {
        isThrown = false;
        isDropped = false;
    }
    protected virtual void HitElseThrown(Collider pCollider)
    {
        if (speedTrail != null) speedTrail.emitting = false;
        isThrown = false;
        isDropped = false;
    }
    protected virtual void HitElseDropped(Collider pCollider)
    {
        isThrown = false;
        isDropped = false;
    }
    protected virtual void Drowns()
    {
        _collider.enabled = false;
        speedTrail.emitting = false;
        _currentPicker = null;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        isThrown = false;
        isDropped = false;
    }

    protected virtual void OnGizmos()
    {        
        if (_currentPicker != null)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Vector3 lPropelPos = new Vector3(0, 0, _proprelZoneCheck.z) - new Vector3(0, 0.5f, 0);
            Gizmos.DrawWireCube(lPropelPos, _proprelZoneCheck*2);
            Gizmos.matrix = Matrix4x4.zero;

            Gizmos.color = Color.red;
            Vector3 lDiretionOffsetThrow = new Vector3(0, Mathf.Sin(THROW_ANGLE_OFFSET * DEG2RAD), 0);
            Gizmos.DrawLine(transform.position, transform.position+(transform.forward + lDiretionOffsetThrow) * 5);
            Gizmos.DrawSphere(transform.position + (transform.forward + lDiretionOffsetThrow) * 5, 0.1f);

            Gizmos.color = Color.magenta;
            Vector3 lDiretionOffsetDrop = new Vector3(0, Mathf.Sin(DROP_ANGLE_OFFSET * DEG2RAD), 0);
            Gizmos.DrawLine(transform.position, transform.position+(transform.forward + lDiretionOffsetDrop) * 3);
            Gizmos.DrawSphere(transform.position + (transform.forward + lDiretionOffsetDrop) * 3, 0.1f);

            Gizmos.color = Color.white;
        }
    }

    private void OnDrawGizmos() { if (_drawGizmos) OnGizmos(); }
}
