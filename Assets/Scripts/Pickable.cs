using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private const float THROW_FORCE = 15000f;

    [Tooltip("This item can be taken")]
    public bool isPickable = true;
    protected private Pickable _currentPicker = null;
    protected private Rigidbody _rb;

    private void Start() => Init();

    protected virtual void Init()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Propel()
    {
        CurrentPicker = null;
        _rb.AddForce(transform.forward * THROW_FORCE);
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
                _rb.isKinematic = false;
                isPickable = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if(CurrentPicker != null && collision.gameObject.layer == )
            CurrentPicker = null;*/
    }
}
