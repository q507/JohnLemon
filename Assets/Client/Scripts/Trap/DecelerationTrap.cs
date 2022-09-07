using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecelerationTrap : MonoBehaviour
{
    public int ID;

    [SerializeField] PlayerController player;
    [SerializeField] SceneData sceneData;

    public DecelerationTrapData decelerationData;

    private void Start()
    {
        decelerationData = new DecelerationTrapData();
        ID = decelerationData.ID;

        sceneData.decelerationDataDic.Add(ID, decelerationData);
    }

    private void Update()
    {
        if (sceneData.decelerationDataDic.TryGetValue(ID, out DecelerationTrapData value))
        {
            sceneData.decelerationDataDic.Remove(ID);
            decelerationData = value;
            sceneData.decelerationDataDic.Add(ID, decelerationData);
        }
        else
        {
            DecelerationTrapData decelerationData = new DecelerationTrapData();
            decelerationData.ID = ID;
            decelerationData.posX = transform.position.x;
            decelerationData.posY = transform.position.y;
            decelerationData.posZ = transform.position.z;
            decelerationData.isSlowDown = this.decelerationData.isSlowDown;
            decelerationData.curCtuSlowTime = this.decelerationData.curCtuSlowTime;
            sceneData.decelerationDataDic.Add(ID, decelerationData);
        }

        _DetectivePlayer();
    }

    private void _DetectivePlayer()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < 2.5f)
        {
            Debug.Log("开始减速");
            decelerationData.isSlowDown = true;
            player.isSlowDown = true;
        }
        if (decelerationData.isSlowDown)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 2.5f)
            {
                StartCoroutine(ContinueSlowDown());
            }
        }
    }

    IEnumerator ContinueSlowDown()
    {
        //同步需要在req中处理
        player.isSlowDown = true;
        yield return new WaitForSeconds(decelerationData.slowContinueTime);
        player.isSlowDown = false;
        decelerationData.isSlowDown = false;
    }
}
