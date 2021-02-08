using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [Tooltip("This item can be taken")]
    public bool isPickable = true;
    protected private Pickable _currentPicker = null;
    protected private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
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
}
