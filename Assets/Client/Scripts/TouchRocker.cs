using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchRocker : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] CanvasGroup touch;

    [SerializeField] PlayerController player;

    //摇杆尺寸大小
    private float smallSize;    
    private float bigSize;

    //摇杆活动半径
    private float radius;

    //遥杆初始位置
    private Vector2 orignPos;

    //记录摇杆的位置
    public float horizontal = 0;
    public float vertical = 0;


    private void Start()
    {
        //TODO:没有点击时将摇杆隐藏（此处有疑问）

        smallSize = ((RectTransform)transform).sizeDelta.x * 0.1f;
        bigSize = ((RectTransform)transform.parent).sizeDelta.x * 0.95f;

        //摇杆活动半径
        radius = bigSize - smallSize;   

        //记录初始位置
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

        //遥感在大圆内
        if (deltaDistance <= radius)
        {
            //使圆内遥感位置等于鼠标位置
            transform.position = eventData.position;
        }
        else
        {
            //如果摇杆位于圆外，遥感位置在大圆边上
            //delta的单位向量 * 半径 + 圆心初始位置 得出在大圆边界上的位置
            transform.position = deltaPos.normalized * radius + orignPos;
        }
        //遥感xy轴对应 stone的xz 形成一种映射 使stone旋转
        //求出弧度
        float tmpAngle = Mathf.Atan2(deltaPos.y, -deltaPos.x);
        tmpAngle = Mathf.Rad2Deg * tmpAngle;
        Vector3 tmpEuler = transform.localEulerAngles;

        tmpEuler.z = tmpAngle;
        //改变z值，使其旋转
        transform.localEulerAngles = tmpEuler;
        player.RotatePlayer(tmpAngle);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //回归原位
        transform.position = orignPos;
    }
}
