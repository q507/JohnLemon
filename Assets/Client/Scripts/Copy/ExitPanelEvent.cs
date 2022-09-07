using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitPanelEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isPointerInside = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPointerInside)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
