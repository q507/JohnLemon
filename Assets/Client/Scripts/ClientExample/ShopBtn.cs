using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBtn : MonoBehaviour
{
    public GameObject ShopPanel;
    // Start is called before the first frame update

    public void OnShopBtnClick()
    {
        ShopPanel.gameObject.SetActive(true);
    }
}
