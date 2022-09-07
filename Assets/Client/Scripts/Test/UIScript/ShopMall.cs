using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopMall : MonoBehaviour
{
    private bool isOpen;    //是否关闭商城详情页

    public delegate void OnShowShopList();  //开启商城目录
    public event OnShowShopList onShowShopList;
    public delegate void DisShowShopList(); //关闭商城目录
    public event DisShowShopList disShowShopList;

    void Start()
    {
        isOpen = false;
        gameObject.GetComponent<Button>().onClick.AddListener(_OnClick);
    }


    private void _OnClick()
    {
        if (!isOpen)
        {
            isOpen = true;
            if (onShowShopList != null)
                onShowShopList();
        }
        else
        {
            isOpen = false;
            if (disShowShopList != null)
                disShowShopList();
        }
    }
}
