using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    private bool isOpen = false;

    public delegate void OnShowShopList();//��������Ŀ¼
    public event OnShowShopList onShowEquipList;
    public delegate void DisShowShopList();//�رձ���Ŀ¼
    public event DisShowShopList disShowEquipList;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(_OnClick);
    }

    private void _OnClick()
    {
        if (!isOpen && onShowEquipList != null)
        {
            onShowEquipList();
            isOpen = true;
        }
        else if (isOpen && disShowEquipList != null)
        {
            disShowEquipList();
            isOpen = false;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
