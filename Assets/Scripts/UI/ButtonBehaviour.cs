using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("Selected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("Selected", false);
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