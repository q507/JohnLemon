using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchRocker : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] CanvasGroup touch;

    [SerializeField] PlayerController player;

    //ҡ�˳ߴ��С
    private float smallSize;    
    private float bigSize;

    //ҡ�˻�뾶
    private float radius;

    //ң�˳�ʼλ��
    private Vector2 orignPos;

    //��¼ҡ�˵�λ��
    public float horizontal = 0;
    public float vertical = 0;


    private void Start()
    {
        //TODO:û�е��ʱ��ҡ�����أ��˴������ʣ�

        smallSize = ((RectTransform)transform).sizeDelta.x * 0.1f;
        bigSize = ((RectTransform)transform.parent).sizeDelta.x * 0.95f;

        //ҡ�˻�뾶
        radius = bigSize - smallSize;   

        //��¼��ʼλ��
        orignPos = transform.position;  
    }

    private void Update()
    {
        horizontal = transform.localPosition.x;
        vertical = transform.localPosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 deltaPos = eventData.position - orignPos;
        float deltaDistance = Vector3.Distance(eventData.position, orignPos);

        //ң���ڴ�Բ��
        if (deltaDistance <= radius)
        {
            //ʹԲ��ң��λ�õ������λ��
            transform.position = eventData.position;
        }
        else
        {
            //���ҡ��λ��Բ�⣬ң��λ���ڴ�Բ����
            //delta�ĵ�λ���� * �뾶 + Բ�ĳ�ʼλ�� �ó��ڴ�Բ�߽��ϵ�λ��
            transform.position = deltaPos.normalized * radius + orignPos;
        }
        //ң��xy���Ӧ stone��xz �γ�һ��ӳ�� ʹstone��ת
        //�������
        float tmpAngle = Mathf.Atan2(deltaPos.y, -deltaPos.x);
        tmpAngle = Mathf.Rad2Deg * tmpAngle;
        Vector3 tmpEuler = transform.localEulerAngles;

        tmpEuler.z = tmpAngle;
        //�ı�zֵ��ʹ����ת
        transform.localEulerAngles = tmpEuler;
        player.RotatePlayer(tmpAngle);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�ع�ԭλ
        transform.position = orignPos;
    }
}
