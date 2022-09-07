using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopList : MonoBehaviour
{
    //private List<RectTransform> childObj = new List<RectTransform>();

    public ShopMall shopMall;

    public GameObject shoppingCanvas;
    public GameObject backpackCanvas;

    public GameObject panelDetail;

    //private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void _OnShow()
    {
        /*if (!shoppingCanvas.gameObject.activeSelf)
        {
            shoppingCanvas.gameObject.SetActive(true);
            backpackCanvas.gameObject.SetActive(false);
        }
        else
        {
            shoppingCanvas.gameObject.SetActive(false);
        }*/
    }

    public void shopDetail()
    {
        //panelDetail.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        //panelDetail.gameObject.SetActive(false);
    }
}
