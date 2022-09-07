using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentClose : MonoBehaviour
{
    public GameObject equipListObj;

    private CanvasGroup equipListCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(_OnClick);
        equipListCanvasGroup = equipListObj.GetComponent<CanvasGroup>();
    }

    private void _OnClick()
    {
        equipListCanvasGroup.alpha = 0;
        equipListCanvasGroup.interactable = false;
        equipListCanvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
