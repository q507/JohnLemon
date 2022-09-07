using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public List<Vector3> pathNodes = new List<Vector3>();

    public bool isRandomCreate;

    private Transform e_transform;

    private int pathNodeNum = 10;

    private void Start()
    {
        e_transform = transform;

        if(isRandomCreate)
        {
            CreateNodes();
        }
    }

    private void CreateNodes()
    {
        //对每个敌人创建十个随机的寻路路径点
        for (int i = 0; i < pathNodeNum; i++)
        {
            float randx = e_transform.position.x + Random.Range(-20, 20);
            float randy = e_transform.position.y + Random.Range(-20, 20);
            float randz = e_transform.position.z + Random.Range(-20, 20);

            Vector3 node = new Vector3(randx, randy, randz);
            pathNodes.Add(node);
        }
    }

    public float GetDistanceToNode(Vector3 origin, int destinationNodeIndex)
    {
        if(destinationNodeIndex < 0 || destinationNodeIndex > pathNodes.Count || pathNodes[destinationNodeIndex] == null)
        {
            return -1f;
        }

        //返回距离下一个巡逻点的距离
        return (pathNodes[destinationNodeIndex] - origin).magnitude;
    }

    public Vector3 GetPositionOfPathNode(int nodeIndex)
    {
        if(nodeIndex < 0 || nodeIndex > pathNodes.Count || pathNodes[nodeIndex] == null)
        {
            return Vector3.zero;
        }

        return pathNodes[nodeIndex];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < pathNodes.Count; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex >= pathNodes.Count)
            {
                nextIndex -= pathNodes.Count;
            }

            Gizmos.DrawLine(pathNodes[i], pathNodes[nextIndex]);
            Gizmos.DrawSphere(pathNodes[i], 0.1f);
        }
    }
}
