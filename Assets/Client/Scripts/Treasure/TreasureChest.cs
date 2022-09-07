using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject highLevelGun;
    [SerializeField] GameObject rightHand;

    [SerializeField] GameObject OpenTip;

    public TreasureChestData chestData;

    public UnityAction OpenChest;

    private void Start()
    {
        chestData = new TreasureChestData();
        OpenTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        _CloseToChest();
        _OpenTreasureChest();
    }

    private void _CloseToChest()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < 1f)
        {
            OpenTip.gameObject.SetActive(true);
        }
        else if(Vector3.Distance(transform.position, player.transform.position) > 1f)
        {
            OpenTip.gameObject.SetActive(false);
        }
    }

    private void _OpenTreasureChest()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenChest?.Invoke();
            OpenTip.gameObject.SetActive(false);
            Instantiate(highLevelGun, rightHand.transform);
        }
    }
}
