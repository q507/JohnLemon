using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopItemDetail : MonoBehaviour, IPointerClickHandler
{
    RectTransform[] trans;

    public ShopMall shopMall;

    public ShoppingMall shoppingMall;

    public ShopSVContent shopSVcontent;

    public NetWork network;

    private string commodityID;

    private string imgPath = "https://media.ufgnix0802.cn/shopIcon/itm030180152.png";

    private void Awake()
    {
        trans = new RectTransform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            trans[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    public void SendBackPackMsg(Backpack.BackpackItem item)
    {
        gameObject.SetActive(true);
        trans[3].gameObject.SetActive(false);

        trans[1].gameObject.GetComponent<Text>().text = "装备名称:" + item.name + "  " + "装备介绍:" + item.introduce + "  "
            + "装备数量:" + item.Amount.ToString();
    }

    public void SendShopMsg(ShoppingMall.CommodityList shopList)
    {
        gameObject.SetActive(true);
        trans[3].gameObject.SetActive(true);

        trans[1].gameObject.GetComponent<Text>().text = "金额:" + shopList.price + "  " + "装备名称:" + shopList.name + "  " + "数量：" 
            + "  " + shopList.amount
            + "装备介绍:" + shopList.introduce;

        commodityID = shopList.commodityID;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            trans[3].gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                //network.ShoppingCommodityReq(commodityID);
            });
        }

    }
}
