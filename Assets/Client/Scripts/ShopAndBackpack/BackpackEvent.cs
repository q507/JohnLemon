using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackEvent : MonoBehaviour
{
    [SerializeField] GameObject backpackListView;
    [SerializeField] GameObject backpackDetail;
    [SerializeField] GameObject shopResources;
    [SerializeField] GameObject shoppingDetail;

    #region 背包相关事件
    public void AppearBackpack()
    {
        if (!backpackListView.gameObject.activeSelf)
        {
            backpackListView.gameObject.SetActive(true);
        }
        else
        {
            backpackListView.gameObject.SetActive(false);
            backpackDetail.gameObject.SetActive(false);
        }

        if (shopResources.gameObject.activeSelf || shopResources.gameObject.activeSelf && shoppingDetail.gameObject.activeSelf)
        {
            shopResources.gameObject.SetActive(false);
            shoppingDetail.gameObject.SetActive(false);
        }
    }

    public void SelectGoods()
    {
        if (!backpackDetail.gameObject.activeSelf)
        {
            backpackDetail.gameObject.SetActive(true);
        }
    }
    #endregion
}
