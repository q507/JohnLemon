using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingEvent : MonoBehaviour
{
    [SerializeField] GameObject shopResources;
    [SerializeField] GameObject backpackListView;
    [SerializeField] GameObject backpackDetail;
    [SerializeField] GameObject shoppingDetail;
    [SerializeField] GameObject confirmationBox;

    private int maxMoney = 9999;
    private int currentMoney = 0;
    private int goodsPrice = 50;

    private void Start()
    {
        currentMoney = maxMoney;
    }

    #region 商店相关事件
    public void AppearShoppingMall()
    {
        if (!shopResources.gameObject.activeSelf)
        {
            shopResources.gameObject.SetActive(true);
        }
        else
        {
            shopResources.gameObject.SetActive(false);
            shoppingDetail.gameObject.SetActive(false);
        }

        if (backpackListView.gameObject.activeSelf || backpackListView.gameObject.activeSelf && backpackDetail.gameObject.activeSelf)
        {
            backpackListView.gameObject.SetActive(false);
            backpackDetail.gameObject.SetActive(false);
        }
    }

    public void SelectGoods()
    {
        if (!shoppingDetail.gameObject.activeSelf)
        {
            shoppingDetail.gameObject.SetActive(true);
        }
    }

    public void Shopping()
    {
        if (shoppingDetail.gameObject.activeSelf)
        {
            confirmationBox.gameObject.SetActive(true);
        }
    }

    public void ConfirmShopping()
    {
        confirmationBox.gameObject.SetActive(false);
        UseMoney();
    }

    public void CancelStopping()
    {
        confirmationBox.gameObject.SetActive(false);
    }

    public void UseMoney()
    {
        currentMoney -= goodsPrice;
        UIManager.Instance.curOwnerMoney.text = "金币：" + currentMoney.ToString();
    }
    #endregion
}
