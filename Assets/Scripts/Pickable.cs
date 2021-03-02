using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;

public class Pickable : MonoBehaviour
{
    private const float THROW_FORCE = 17f;

    private Vector3 _forwardVelocity;
    private Vector3 _diretionOffset = new Vector3(0,-0.1f);

    protected private PlayerController _currentPicker = null;
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

    public PlayerController CurrentPicker
    {
        get => _currentPicker;
        set
        {
            if (value != null) GetPicked();
            else GetDropped();
            
            _currentPicker = value;
        }
    }

    protected private virtual void GetPicked()
    {
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        _collider.enabled = false;
        _rb.isKinematic = true;
        isPickable = false;
    }

    protected private virtual void GetDropped()
    {
        transform.parent = null;
        _collider.enabled = true;
        _rb.isKinematic = false;
        isPickable = true;
    }
}
