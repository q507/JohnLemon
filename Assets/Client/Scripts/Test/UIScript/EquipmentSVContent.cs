using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSVContent : MonoBehaviour
{
    private List<RectTransform> itemList = new List<RectTransform>();

    public GameObject buttonPref;

    // Start is called before the first frame update
    void Start()
    {
        _InitUI(25);
    }

    private void _InitUI(int initCount)
    {
        UITool.GetInstance().InitButtonPrefab(buttonPref, gameObject.transform, _OnClick, ref itemList, initCount);
        Transform text;
        for (int i = 0; i < initCount; i++)
        {
            text = itemList[i].transform.Find("Text (Legacy)");
            text.gameObject.GetComponent<Text>().text = Random.Range(0, 99999).ToString();
        }

    }


    private void _OnClick()
    {
        Debug.Log("点击对象");
        //GameObject curItem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;//获取当前点击对象
        //if (itemContent[curItem] == null)
        //{
        //    Debug.Log("null");
        //}
        //shopDetail.SendMsg(itemContent[curItem]);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
