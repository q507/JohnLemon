using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITool
{
    private static UITool instance;

    private UITool() { }

    public static UITool GetInstance()
    {
        if (instance == null)
        {
            instance = new UITool();
        }
        return instance;
    }


    public bool InitButtonPrefab(GameObject button, Transform parentTrans, UnityEngine.Events.UnityAction callFunc, ref List<RectTransform> lists, int initCount)
    {
        Button newbutton;
        if (!button.TryGetComponent<Button>(out newbutton))
        {
            Debug.Log("你这按钮有问题啊");
            return false;
        }

        if (button == null || callFunc == null || initCount <= 0)
            return false;


        float offestX = 105;
        float offestY = -105;
        float iniX = 70;
        float iniY = -70;
        float maxLengthX = 280;
        Vector2 buttonSize = new Vector2(100, 100);

        GameObject buttonObj = null;
        RectTransform t = null;
        for (int i = 0; i < initCount; i++)
        {
            buttonObj = GameObject.Instantiate(button, parentTrans);
            buttonObj.transform.name = "Button" + lists.Count;
            if (lists.Count == 0)
            {
                buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(iniX, iniY);
            }
            else
            {
                t = lists[lists.Count - 1];
                if (t.anchoredPosition.x >= maxLengthX)
                {
                    buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(iniX, t.anchoredPosition.y + offestY);
                }
                else
                    buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(t.anchoredPosition.x + offestX, t.anchoredPosition.y);
            }
            lists.Add(buttonObj.GetComponent<RectTransform>());
            buttonObj.GetComponent<RectTransform>().sizeDelta = buttonSize;
            buttonObj.GetComponent<Button>().onClick.AddListener(callFunc);
        }

        return true;
    }

}
