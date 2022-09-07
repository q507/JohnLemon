using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ScrollViewManager : MonoBehaviour
{
    [SerializeField] GameObject productImage;
    [SerializeField] GameObject shoppingDetail;

    //创建两个字典分别存储商品的图片信息以及详细说明
    private Dictionary<RectTransform, Image> imgDic = new Dictionary<RectTransform, Image>();
    private Dictionary<RectTransform, string> itemDic = new Dictionary<RectTransform, string>();

    void Start()
    {
        //查找Content目录下所有的商品
        RectTransform[] resources = new RectTransform[transform.childCount];
        //遍历所有的商品
        for (int i = 0; i < transform.childCount; i++)
        {
            resources[i] = transform.GetChild(i) as RectTransform;
        }

        //将所有的商品图片添加进字典中
        foreach (RectTransform image in resources)
        {
            imgDic.Add(image, image.GetComponent<Image>());

            image.GetComponent<Button>().onClick.AddListener(() => {
                ClickEvent(image);
            });
        }

        //将所有的商品信息添加进字典中
        foreach (RectTransform explain in resources)
        {
            itemDic.Add(explain, explain.transform.gameObject.name + Environment.NewLine + Environment.NewLine + "这个物品很好，孩子很喜欢吃，下次还会回购！");

            explain.GetComponent<Button>().onClick.AddListener(()=>{
                ClickEvent(explain);
            });
        }
        
    }

    /// <summary>
    /// 商品按钮点击事件，展示每个按钮所对应的图片以及详细说明
    /// </summary>
    private void ClickEvent(RectTransform rect)
    {
        productImage.transform.GetComponent<Image>().sprite = imgDic[rect].sprite;
        shoppingDetail.transform.GetComponent<Text>().text = itemDic[rect];
    }
}
