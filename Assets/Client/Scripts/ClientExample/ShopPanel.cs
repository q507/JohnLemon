using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class ItemInfo
{
    public string name;
    public string introduce;
    public int price;
    public string iconName;
}

[Serializable]
public class ItemInfos
{
    public List<ItemInfo> items;
}

public class ItemPrefab
{
    public GameObject go;
    public Image itemImage;
}

public class ShopPanel : MonoBehaviour
{
    public Button buyButton;

    public Text moneyNumText;

    public GameObject itemPrefab;

    public GameObject itemContent;

    public GameObject itemInfoPanel;

    public Text itemInfoText;

    public Image itemInfoImage;

    public MsgBox msgBox;

    public Button closeBtn;

    private Dictionary<string, Sprite> itemSpriteDict;

    private int moneyNum;

    private ItemInfo currentSelect;

    private void Start()
    {
        InitItemAtlas();
        InitItemList();
    }

    private void OnEnable()
    {
        moneyNum = 10000;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyClick);
        
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(delegate { gameObject.SetActive(false); });
        moneyNumText.text = moneyNum.ToString();

        currentSelect = null;
        itemInfoPanel.gameObject.SetActive(false);
    }

    private void OnBuyClick()
    {
        if (currentSelect == null)
        {
            Debug.Log("请选中需要购买的物品");
            return;
        }

        if (currentSelect.price > moneyNum)
        {
            Debug.Log("钱不够");
            return;
        }
        
        msgBox.gameObject.SetActive(true);
        string msg = $"是否花费{currentSelect.price}金币购买{currentSelect.name}";
        msgBox.SetData(msg,delegate { 
            Debug.Log("购买成功"); 
            moneyNum -= currentSelect.price;
            moneyNumText.text = moneyNum.ToString();
            currentSelect = null;
            itemInfoPanel.gameObject.SetActive(false);
        });
    }

    private void InitItemList()
    {
        TextAsset itemConfig = Resources.Load<TextAsset>("ItemConfig");
        if (itemConfig != null)
        {
            ItemInfos itemInfos = JsonUtility.FromJson<ItemInfos>(itemConfig.text);
            foreach (var itemInfo in itemInfos.items)
            {
                ItemPrefab item = new ItemPrefab();
                item.go = Instantiate<GameObject>(itemPrefab,itemContent.transform);
                Sprite sp;
                itemSpriteDict.TryGetValue(itemInfo.iconName, out sp);
                item.itemImage = item.go.transform.Find("ItemImage").GetComponent<Image>();
                item.itemImage.sprite = sp;
                item.go.SetActive(true);
                item.go.transform.GetComponent<Button>().onClick.AddListener(delegate
                {
                    currentSelect = itemInfo;
                    itemInfoPanel.gameObject.SetActive(true);
                    itemInfoText.text = itemInfo.introduce;
                    itemInfoImage.sprite = sp;
                });
            }
        }
    }

    private void InitItemAtlas()
    {
        UnityEngine.Object[] objs = Resources.LoadAll("itemIcon");
        itemSpriteDict = new Dictionary<string, Sprite>();
        foreach (var obj in objs)
        {
            if (obj.GetType() == typeof(Sprite))
            {
                if(!itemSpriteDict.ContainsKey(obj.name))
                    itemSpriteDict.Add(obj.name,(Sprite)obj);
            }
            else
            {
                Debug.Log($"转换失败{obj.name},{obj.GetType().Name}");
            }
            
        }
    }
}
