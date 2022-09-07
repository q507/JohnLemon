using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler 
{
    [SerializeField] Animator customButtonAnimator;
    [SerializeField] GameObject roleObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            customButtonAnimator.SetBool("isTouch", true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            customButtonAnimator.SetTrigger("isClick");

            if (!roleObject.gameObject.activeSelf)
            {
                roleObject.gameObject.SetActive(true);
            }
            else
            {
                roleObject.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            customButtonAnimator.SetBool("isTouch", false);
        }
    }
}
