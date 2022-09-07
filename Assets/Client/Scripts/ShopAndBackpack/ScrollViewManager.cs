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

    //���������ֵ�ֱ�洢��Ʒ��ͼƬ��Ϣ�Լ���ϸ˵��
    private Dictionary<RectTransform, Image> imgDic = new Dictionary<RectTransform, Image>();
    private Dictionary<RectTransform, string> itemDic = new Dictionary<RectTransform, string>();

    void Start()
    {
        //����ContentĿ¼�����е���Ʒ
        RectTransform[] resources = new RectTransform[transform.childCount];
        //�������е���Ʒ
        for (int i = 0; i < transform.childCount; i++)
        {
            resources[i] = transform.GetChild(i) as RectTransform;
        }

        //�����е���ƷͼƬ��ӽ��ֵ���
        foreach (RectTransform image in resources)
        {
            imgDic.Add(image, image.GetComponent<Image>());

            image.GetComponent<Button>().onClick.AddListener(() => {
                ClickEvent(image);
            });
        }

        //�����е���Ʒ��Ϣ��ӽ��ֵ���
        foreach (RectTransform explain in resources)
        {
            itemDic.Add(explain, explain.transform.gameObject.name + Environment.NewLine + Environment.NewLine + "�����Ʒ�ܺã����Ӻ�ϲ���ԣ��´λ���ع���");

            explain.GetComponent<Button>().onClick.AddListener(()=>{
                ClickEvent(explain);
            });
        }
        
    }

    /// <summary>
    /// ��Ʒ��ť����¼���չʾÿ����ť����Ӧ��ͼƬ�Լ���ϸ˵��
    /// </summary>
    private void ClickEvent(RectTransform rect)
    {
        productImage.transform.GetComponent<Image>().sprite = imgDic[rect].sprite;
        shoppingDetail.transform.GetComponent<Text>().text = itemDic[rect];
    }
}
