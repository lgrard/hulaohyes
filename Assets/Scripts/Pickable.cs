using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;
using hulaohyes.enemy;

public class Pickable : MonoBehaviour
{
    private const float THROW_FORCE = 20f;
    private const float DROP_FORCE = 9f;

    private LayerMask _collisionLayer;
    private Vector3 _forwardVelocity;
    private Vector3 _diretionOffset = new Vector3(0,-0.15f);

    protected private PlayerController _currentPicker = null;
    protected private Rigidbody _rb;
    protected private Collider _collider;

    public bool isPickable => _currentPicker == null;

    private void Start() => Init();

    protected virtual void Init()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _collisionLayer = LayerMask.GetMask("Enemy","Player","KillZone","Default","Bricks","Ground");
    }

    public virtual void Propel() => GetDropped(THROW_FORCE);
    public virtual void Drop() => GetDropped(DROP_FORCE);

    public virtual void GetPicked(PlayerController pPicker)
    {
        transform.parent = pPicker.pickUpPoint;
        _currentPicker = pPicker;
        _rb.isKinematic = true;

        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        _collider.isTrigger = true;
    }

    private void GetDropped(float pDropForce)
    {
        _currentPicker.DropTarget();
        transform.parent = null;
        _currentPicker = null;
        _rb.isKinematic = false;
        _forwardVelocity = (transform.forward - _diretionOffset) * pDropForce;
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
}
