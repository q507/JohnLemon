using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMathAABB
{
    public Vector3 MinVector 
    { 
        get; 
    }

    public Vector3 MaxVector 
    { 
        get; 
    }

    public Vector3 Center 
    { 
        get; 
    }

    public Vector3[] Corners 
    { 
        get; 
    }

    //��ȡ���ĵ�
    public Vector3 GetCenter();

    //��ȡ��Χ�а˸�������Ϣ
    public void GetCorners();

    //�ж�������Χ���Ƿ���ײ
    public bool Intersects(IMathAABB aabb);

    //����������Ƿ��ڰ�Χ����
    public bool ContainPoint(Vector3 point);

    //����һ���µİ�Χ�� ͬʱ����������Χ�У��µİ�Χ��: min����Ҫ������������С���Ǹ���max����Ҫ���������������Ǹ�
    public void Merge(IMathAABB box);

    //����
    public void SetMinMax(Vector3 min, Vector3 max);

    //����
    public void ResetMinMax();

    public bool IsEmpty();
}
