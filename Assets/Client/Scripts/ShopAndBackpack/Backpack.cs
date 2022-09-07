using System;
using System.Collections;
using System.Collections.Generic;
using TCCamp;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    [SerializeField] GameObject backpackContent;
    [SerializeField] NetWork network;
    [SerializeField] GameObject commodityPrefab;

    public struct BackpackItem
    {
        public string commodityID;
        public string name;
        public string introduce;
        private int amount;

        public int Amount { get => amount; set => amount = value; }
    }

    private Dictionary<string, BackpackItem> backpackDic = new Dictionary<string, BackpackItem>();
    private Dictionary<RectTransform, Button> backPacksUI = new Dictionary<RectTransform, Button>();


    public void InitBackPackUI(string ID)
    {
        GameObject obj = Instantiate(commodityPrefab, transform);
        obj.GetComponent<Button>().GetComponentInChildren<Text>().text = ID.ToString();
        backPacksUI.Add(obj.GetComponent<RectTransform>(), obj.GetComponent<Button>());
    }

    private void _UpdateBackPackObj(Button button)
    {
        if (backPacksUI.TryGetValue(button.gameObject.GetComponent<RectTransform>(), out Button item))
            return;

        backPacksUI.Add(button.gameObject.GetComponent<RectTransform>(), button);
    }

    //public void InitBackPack(ShoppingMallRsp rsp)
    //{
    //    foreach (var value in rsp.CommodityData)
    //    {
    //        BackpackItem item = new BackpackItem();
    //        item.commodityID = value.CommodityID;
    //        item.name = value.Name;
    //        item.introduce = value.Introduce;
    //        item.Amount = value.Amount;

    //        backpackDic.Add(value.CommodityID, item);
    //        InitBackPackUI(value.CommodityID);
    //    }
    //}

    public void AddBackPack(ShoppingMall.CommodityList commodityItem)
    {
        if (backpackDic.TryGetValue(commodityItem.commodityID, out BackpackItem value))
        {
            backpackDic.Remove(commodityItem.commodityID);
            BackpackItem item = new BackpackItem();
            item = value;
            item.Amount += 1;
            backpackDic.Add(commodityItem.commodityID, item);
        }
        else
        {
            BackpackItem item = new BackpackItem();
            item.commodityID = commodityItem.commodityID;
            item.name = commodityItem.name;
            item.introduce = commodityItem.introduce;
            item.Amount = 1;
            backpackDic.Add(commodityItem.commodityID, item);

            //¸üÐÂUI
            GameObject obj = Instantiate(commodityPrefab, transform);
            Button objButton = obj.GetComponent<Button>();
            objButton.GetComponentInChildren<Text>().text = commodityItem.commodityID.ToString();
            _UpdateBackPackObj(objButton);
        }
    }
}
