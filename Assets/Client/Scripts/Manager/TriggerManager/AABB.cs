using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : MonoBehaviour, IMathAABB
{
    //�޸Ĵ�ֵ����m_CalcMin
    [SerializeField]
    private Vector3 minVector = Vector3.zero;

    //�޸Ĵ�ֵ����m_CalcMax
    [SerializeField]
    private Vector3 maxVector = Vector3.zero;

    [SerializeField]
    private Vector3 centerVector = Vector3.zero;

    //�����Χ�а˸�����
    private Vector3[] corners = new Vector3[8];

    public Vector3 MinVector
    {
        get
        {
            return realCalcMin;
        }
    }

    public Vector3 MaxVector
    {
        get
        {
            return realCalcMax;
        }
    }

    public Vector3[] Corners
    {
        get
        {
            return corners;
        }
    }

    public Vector3 Center
    {
        get
        {
            return centerVector;
        }
    }

    /// <summary>
    /// ʵ�ʼ������Сֵ
    /// </summary>
    private Vector3 realCalcMin;

    /// <summary>
    /// ʵ�ʼ�������ֵ
    /// </summary>
    private Vector3 realCalcMax;

    /// <summary>
    /// ��ֹ��update֮ǰ������ײ
    /// </summary>
    private void Awake()
    {
        _UpdatePosition();
    }

    private void Start()
    {
        TriggerManager.Instance.AddInterecters(this);

        minVector = new Vector3(-transform.gameObject.GetComponent<Renderer>().bounds.size.x * 0.5f, -transform.gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f, -transform.gameObject.GetComponent<Renderer>().bounds.size.z * 0.5f);
        maxVector = new Vector3(transform.gameObject.GetComponent<Renderer>().bounds.size.x * 0.5f, transform.gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f, transform.gameObject.GetComponent<Renderer>().bounds.size.z * 0.5f);
    }

    private void Update()
    {
        _UpdatePosition();

        DrawRect();
    }

    /// <summary>
    /// ����λ��
    /// </summary>
    private void _UpdatePosition()
    {
        // position
        SetMinMax(minVector + transform.position, maxVector + transform.position);
    }

    public Vector3 GetCenter()
    {
        centerVector.x = 0.5f * (realCalcMin.x + realCalcMax.x);
        centerVector.y = 0.5f * (realCalcMin.y + realCalcMax.y);
        centerVector.z = 0.5f * (realCalcMin.z + realCalcMax.z);
        return centerVector;
    }

    public void GetCorners()
    {
        // ����Z�����������
        // ���϶�������
        corners[0].Set(realCalcMin.x, realCalcMax.y, realCalcMax.z);
        // ���¶�������
        corners[1].Set(realCalcMin.x, realCalcMin.y, realCalcMax.z);
        // ���¶�������
        corners[2].Set(realCalcMax.x, realCalcMin.y, realCalcMax.z);
        // ���϶�������
        corners[3].Set(realCalcMax.x, realCalcMax.y, realCalcMax.z);

        // ����Z�Ḻ�������
        // ���϶�������
        corners[4].Set(realCalcMax.x, realCalcMax.y, realCalcMin.z);
        // ���¶�������.
        corners[5].Set(realCalcMax.x, realCalcMin.y, realCalcMin.z);
        // ���¶�������.
        corners[6].Set(realCalcMin.x, realCalcMin.y, realCalcMin.z);
        // ���϶�������.
        corners[7].Set(realCalcMin.x, realCalcMax.y, realCalcMin.z);
    }

    public void DrawRect()
    {
        Debug.DrawLine(Corners[0], Corners[1], Color.red);
        Debug.DrawLine(Corners[1], Corners[2], Color.red);
        Debug.DrawLine(Corners[2], Corners[3], Color.red);
        Debug.DrawLine(Corners[3], Corners[0], Color.red);

        Debug.DrawLine(Corners[4], Corners[5], Color.red);
        Debug.DrawLine(Corners[5], Corners[6], Color.red);
        Debug.DrawLine(Corners[6], Corners[7], Color.red);
        Debug.DrawLine(Corners[7], Corners[4], Color.red);

        Debug.DrawLine(Corners[0], Corners[7], Color.red);
        Debug.DrawLine(Corners[1], Corners[6], Color.red);
        Debug.DrawLine(Corners[2], Corners[5], Color.red);
        Debug.DrawLine(Corners[3], Corners[4], Color.red);
    }

    public bool Intersects(IMathAABB aabb)
    {
        //���ụ���Ƿ��������aabb ������ǰ��Χ�У�||����ǰ�İ�Χ�а��� aabb��
        return ((realCalcMin.x >= aabb.MinVector.x && realCalcMin.x <= aabb.MaxVector.x) || (aabb.MinVector.x >= realCalcMin.x && aabb.MinVector.x <= realCalcMax.x)) &&
               ((realCalcMin.y >= aabb.MinVector.y && realCalcMin.y <= aabb.MaxVector.y) || (aabb.MinVector.y >= realCalcMin.y && aabb.MinVector.y <= realCalcMax.y)) &&
               ((realCalcMin.z >= aabb.MinVector.z && realCalcMin.z <= aabb.MaxVector.z) || (aabb.MinVector.z >= realCalcMin.z && aabb.MinVector.z <= realCalcMax.z));
    }

    public bool ContainPoint(Vector3 point)
    {
        if (point.x < realCalcMin.x) return false;
        if (point.y < realCalcMin.y) return false;
        if (point.z < realCalcMin.z) return false;
        if (point.x > realCalcMax.x) return false;
        if (point.y > realCalcMax.y) return false;
        if (point.z > realCalcMax.z) return false;
        return true;
    }

    //���������С����
    public void Merge(IMathAABB box)
    {
        // �����µ���С������
        realCalcMin.x = Mathf.Min(realCalcMin.x, box.MinVector.x);
        realCalcMin.y = Mathf.Min(realCalcMin.y, box.MinVector.y);
        realCalcMin.z = Mathf.Min(realCalcMin.z, box.MinVector.z);

        // �����µ���������
        realCalcMax.x = Mathf.Max(realCalcMax.x, box.MaxVector.x);
        realCalcMax.y = Mathf.Max(realCalcMax.y, box.MaxVector.y);
        realCalcMax.z = Mathf.Max(realCalcMax.z, box.MaxVector.z);

        GetCenter();
        GetCorners();
    }

    public void SetMinMax(Vector3 min, Vector3 max)
    {
        this.realCalcMin = min;
        this.realCalcMax = max;
        GetCenter();
        GetCorners();
    }

    public bool IsEmpty()
    {
        return realCalcMin.x > realCalcMax.x || realCalcMin.y > realCalcMax.y || realCalcMin.z > realCalcMax.z;
    }

    public void ResetMinMax()
    {
        realCalcMin.Set(-transform.gameObject.GetComponent<Renderer>().bounds.size.x * 0.5f, -transform.gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f, -transform.gameObject.GetComponent<Renderer>().bounds.size.z * 0.5f);
        realCalcMax.Set(transform.gameObject.GetComponent<Renderer>().bounds.size.x * 0.5f, transform.gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f, transform.gameObject.GetComponent<Renderer>().bounds.size.z * 0.5f);
        GetCenter();
        GetCorners();
    }
}
