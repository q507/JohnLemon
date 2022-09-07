using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TCCamp;

public class ShoppingMall : MonoBehaviour
{
    [SerializeField] GameObject backpackContent;
    [SerializeField] NetWork network;
    [SerializeField] GameObject commodityPrefab;

    private string imgPath = "https://media.ufgnix0802.cn/shopIcon/itm010100011.png";

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

    private bool isFirstBuy = true;

    public Dictionary<string, CommodityList> shopListDic = new Dictionary<string, CommodityList>();
    private Dictionary<RectTransform, Button> shopListUIDic = new Dictionary<RectTransform, Button>();

    private void InitUI()
    {
        foreach (var shopItemUI in shopListDic)
        {
            var item = Instantiate(commodityPrefab, transform);
            item.GetComponentInChildren<Text>().text = shopItemUI.Value.commodityID;
            StartCoroutine(InitImg(item));
            shopListUIDic.Add(item.GetComponent<RectTransform>(), item.GetComponent<Button>());
        }
    }

    private void _AddUI(string ID)
    {
        var item = Instantiate(commodityPrefab, transform);
        item.GetComponentInChildren<Text>().text = ID.ToString();
        StartCoroutine(InitImg(item));
        shopListUIDic.Add(item.GetComponent<RectTransform>(), item.GetComponent<Button>());
    }

    //初始化商品列表
    //public void InitShopList(ShoppingSyncRsp shopListData)
    //{
    //    foreach (var item in shopListData.CommodityData)
    //    {
    //        if (shopListDic.TryGetValue(item.CommodityID, out CommodityList value))
    //        {
    //            value.commodityID = item.CommodityID;
    //            value.name = item.Name;
    //            value.price = item.Price;
    //            value.introduce = item.Introduce;
    //            value.amount = item.Amount;
    //        }
    //        else
    //        {
    //            CommodityList commodity = new CommodityList();
    //            commodity.commodityID = item.CommodityID;
    //            commodity.name = item.Name;
    //            commodity.price = item.Price;
    //            commodity.introduce = item.Introduce;
    //            commodity.amount = item.Amount;
    //            //更新UI
    //            if (!isFirstBuy)
    //            {
    //                _AddUI(item.CommodityID);
    //            }
    //        }
    //    }

    //    if (isFirstBuy)//是否为启动程序UI渲染
    //    {
    //        InitUI();
    //        isFirstBuy = false;
    //    }
    //}

    public IEnumerator InitImg(GameObject item)
    {
        UnityWebRequest webrequest = UnityWebRequest.Get(imgPath);

        DownloadHandlerTexture tx = new DownloadHandlerTexture();
        webrequest.downloadHandler = tx;

        yield return webrequest.SendWebRequest();
        if (webrequest.isDone)
        {
            Texture2D tex1 = new Texture2D(84, 84);
            tex1 = tx.texture;
            Sprite sprite = Sprite.Create(tex1, new Rect(0, 0, 84, 84), new Vector2(0.5f, 0.5f));

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = sprite;
            }
        }
    }
}
