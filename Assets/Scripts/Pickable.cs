using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private const float THROW_FORCE = 17f;

    private Vector3 _forwardVelocity;
    private Vector3 _diretionOffset = new Vector3(0,-0.1f);

    protected private Pickable _currentPicker = null;
    protected private Rigidbody _rb;
    protected private Collider _collider;

    [HideInInspector]
    public bool isPickable = true;

    private void Start() => Init();

    protected virtual void Init()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public virtual void Propel()
    {
        CurrentPicker = null;
        _forwardVelocity = (transform.forward - _diretionOffset) * THROW_FORCE;
        _rb.velocity = _forwardVelocity;
    }

    public Pickable CurrentPicker
    {
        get => _currentPicker;
        set
        {
            _currentPicker = value;

            if (_currentPicker != null)
            {
                transform.parent = _currentPicker.transform;
                transform.localEulerAngles = Vector3.zero;
                _rb.isKinematic = true;
                isPickable = false;
            }

            else
            {
                transform.parent = null;
                _collider.isTrigger = false;
                _rb.isKinematic = false;
                isPickable = true;
            }
        }
    }

    public virtual void GetPicked()
    {
        _collider.isTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if(CurrentPicker != null && collision.gameObject.layer == )
            CurrentPicker = null;*/
    }
}
