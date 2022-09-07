using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��̬������Ϊ
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

    //Ѳ���¼�
    private void _CheckTarget()
    {
        //�ӹ��������ƫ�´�����һ���������ڼ����ң�ģ�������ֵ�Ͳ
        if (Physics.Raycast(transform.position + auxiliaryEmissionRayPosition, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, gargoyleData.patrolDistance, playerLayer))
        {
            if (Vector3.Distance(hitInfo.point, player.transform.position) < gargoyleData.patrolDistance)
            {
                //�����￴�����ʱ����������1���˺�
                player.Hurt(gargoyleData.hurtNum);
            }
        }
    }
}
