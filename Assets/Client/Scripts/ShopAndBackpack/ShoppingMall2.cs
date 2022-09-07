using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingMal2 : MonoBehaviour
{
    [SerializeField] GameObject backpackContent;

    public struct CommodityList
    {
        public string commodityID;
        public string name;
        public string introduce;
        public int amount;
        public int price;
    }


    private string shopsID;
    private string shopsAmount;

    private Dictionary<RectTransform, Button> shopsBtnDic = new Dictionary<RectTransform, Button>();
    private Dictionary<RectTransform, Text> shopsTxtDic = new Dictionary<RectTransform, Text>();

    private void Start()
    {
        RectTransform[] shops = new RectTransform[transform.childCount];
        RectTransform[] amount = new RectTransform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            shops[i] = transform.GetChild(i) as RectTransform;
            amount[i] = shops[i].transform.GetChild(0) as RectTransform;
        }

        foreach (RectTransform shopsBtn in shops)
        {
            shopsBtnDic.Add(shopsBtn, shopsBtn.GetComponent<Button>());

            shopsBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                //判断ID相等商品数量相加事件
                shopsID = shopsBtn.GetComponentInChildren<Text>().text;
                ClickEvent(shopsBtn);
            });
        }

        foreach (RectTransform shopsText in amount)
        {
            shopsTxtDic.Add(shopsText, shopsText.GetComponent<Text>());

            shopsText.GetComponentInParent<Button>().onClick.AddListener(() =>
            {
                DisplayShopsTextEvent(shopsText.GetComponent<Text>().text);
            });
        }
    }

    private void ClickEvent(RectTransform button)
    {
        Instantiate(button, backpackContent.transform);
        Debug.Log("该商品的商品ID为：" + shopsID);
    }

    private void DisplayShopsTextEvent(string text)
    {
        Debug.Log("按钮文本为：" + text);
    }
}
