using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopMall : MonoBehaviour
{
    private bool isOpen;    //�Ƿ�ر��̳�����ҳ

    public delegate void OnShowShopList();  //�����̳�Ŀ¼
    public event OnShowShopList onShowShopList;
    public delegate void DisShowShopList(); //�ر��̳�Ŀ¼
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
