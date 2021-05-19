using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    private Animator anim;
    private EventSystem eventSystem;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        eventSystem = EventSystem.current;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("Selected", true);
        eventSystem.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        anim.SetBool("Selected", true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        anim.SetBool("Selected", false);
    }
}