using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 静态怪物行为
/// </summary>
public enum GargoyleState
{
    Idle
}

public class Gargoyle : MonoBehaviour
{
    [SerializeField] PlayerController player;

    [SerializeField] LayerMask playerLayer;

    public GargoyleData gargoyleData;

    private GargoyleState gargoyleState;

    private Vector3 auxiliaryEmissionRayPosition = new Vector3(0, 1f, 0);

    public GargoyleState GargoyleState { get => gargoyleState;
        set {
            gargoyleState = value;
            switch (gargoyleState)
            {
                case GargoyleState.Idle:
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        gargoyleData = new GargoyleData();

        gargoyleState = GargoyleState.Idle;
    }

    private void Update()
    {
        switch (gargoyleState)
        {
            case GargoyleState.Idle:
                _CheckTarget();
                break;
            default:
                break;
        }
    }

    //巡逻事件
    private void _CheckTarget()
    {
        //从怪物的中心偏下处发射一条射线用于检测玩家，模拟怪物的手电筒
        if (Physics.Raycast(transform.position + auxiliaryEmissionRayPosition, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, gargoyleData.patrolDistance, playerLayer))
        {
            if (Vector3.Distance(hitInfo.point, player.transform.position) < gargoyleData.patrolDistance)
            {
                //当怪物看到玩家时，对玩家造成1点伤害
                player.Hurt(gargoyleData.hurtNum);
            }
        }
    }
}
