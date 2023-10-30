using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIExpand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator anim;

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.Play("Expand");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.Play("Contract");
    }
}
