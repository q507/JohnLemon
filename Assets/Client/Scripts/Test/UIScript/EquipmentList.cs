using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentList : MonoBehaviour
{
    public Equipment equipment;

    //private CanvasGroup canvasGroup;
    public GameObject backpackCanvas;
    public GameObject shoppingCanvas;
    public GameObject panelDetail;


    // Start is called before the first frame update
    void Start()
    {
        /*equipment.onShowEquipList += _OnShow;
        equipment.disShowEquipList += _DisShow;*/

        //canvasGroup = GetComponent<CanvasGroup>();
      //  _DisShow();
    }

    public void _OnShow()
    {
        if (!backpackCanvas.gameObject.activeSelf)
        {
            backpackCanvas.gameObject.SetActive(true);
            shoppingCanvas.gameObject.SetActive(false);
        }
        else
        {
            backpackCanvas.gameObject.SetActive(false);
        }

        /*canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;*/
    }

    public void equipmentDetail()
    {
        panelDetail.gameObject.SetActive(true);
    }

    public void CloseEquipment()
    {
        panelDetail.gameObject.SetActive(false);
    }

    private void _DisShow()
    {
        /*backpackCanvas.gameObject.SetActive(false);
        shoppingCanvas.gameObject.SetActive(true);*/
        /*canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
