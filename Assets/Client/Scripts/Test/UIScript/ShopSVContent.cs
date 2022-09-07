using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopSVContent : MonoBehaviour
{
    private List<RectTransform> itemList = new List<RectTransform>();

    private Dictionary<GameObject, string> itemContent = new Dictionary<GameObject, string>();

    public GameObject buttonPref;

    public GameObject shopping;
    public GameObject backpack;

    //声明接口
    /*public delegate void ShopItemSendMsg(string msg);//开启商城物品详情页
    public event ShopItemSendMsg shopItemSendShopMsg;

    public delegate void BackpackItemSendMsg(string msg);//开启商城物品详情页
    public event BackpackItemSendMsg backpackItemSendMsg;*/


    // Start is called before the first frame update
    void Start()
    {
        _InitUI(15);
    }

    private void _InitUI(int initCount)
    {
        UITool.GetInstance().InitButtonPrefab(buttonPref, gameObject.transform, _OnClick, ref itemList, initCount);
        for (int i = 0; i < initCount; i++)
        {
            itemContent.Add(itemList[i].gameObject, "测试物体：" + itemList[i].transform.name);
        }
    }

    private void _OnClick()
    {
        GameObject curItem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;//获取当前点击对象
        if (!itemContent.TryGetValue(curItem, out string msg))
        {
            Debug.Log("null");
            return;
        }
        /*if (shopping.gameObject.activeSelf && shopItemSendShopMsg != null)
        {
            shopItemSendShopMsg(itemContent[curItem]);
        }
        else if (backpack.gameObject.activeSelf && backpackItemSendMsg != null)
        {
            backpackItemSendMsg(itemContent[curItem]);
        }*/
    }



    // Update is called once per frame
    void Update()
    {

    }
}
