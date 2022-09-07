 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public static TriggerManager Instance;

    List<IMathAABB> triggerObj = new List<IMathAABB>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _TriggerEvent();
    }

    public void AddInterecters(IMathAABB aabb)
    {
        triggerObj.Add(aabb);
    }

    private void _TriggerEvent()
    {
        for (int i = 0; i < triggerObj.Count; i++)
        {
            for (int j = i + 1; j < triggerObj.Count; j++)
            {
                if (triggerObj[i].Intersects(triggerObj[j]))
                {
                    Debug.Log("·¢ÉúÅö×²");
                }
            }
        }
    }
}
