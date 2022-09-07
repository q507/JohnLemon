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

    //获取中心点
    public Vector3 GetCenter();

    //获取包围盒八个顶点信息
    public void GetCorners();

    //判断两个包围盒是否碰撞
    public bool Intersects(IMathAABB aabb);

    //返回这个点是否在包围盒中
    public bool ContainPoint(Vector3 point);

    //生成一个新的包围盒 同时容纳两个包围盒，新的包围盒: min各轴要是其他两个最小的那个，max各轴要是其他两个最大的那个
    public void Merge(IMathAABB box);

    //设置
    public void SetMinMax(Vector3 min, Vector3 max);

    //重置
    public void ResetMinMax();

    public bool IsEmpty();
}
