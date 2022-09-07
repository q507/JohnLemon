using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定义所有AI物体使用到的状态和转换
//转换
public enum Transition
{
   SawPlayer = 0,   //看到玩家
   ReachPlayer,     //接近玩家
   LostPlayer,      //玩家离开视线范围
   AttackPlayer,    //攻击角色
   NoHealth,        //死亡
   Relax,           //休息
   Work             //休息结束
}

//状态
public enum FSMStateID
{
    patrolling = 0, //巡逻状态
    Chasing,        //追逐状态
    Attacking,      //攻击状态
    Dead,           //死亡状态
    Idle            //站立状态
}

public abstract class FSMState
{
    //用于存储 “转化-状态” 键值对
    protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();

    //状态编号ID
    protected FSMStateID stateID;

    public FSMStateID ID { get { return stateID; } }

    //Reson方法用于确定是否需要转换到其他状态，应该发生哪个转换
    public abstract void Reason();

    //Act方法定义了在本状态中角色的行为，如行动、动画等
    public abstract void Act();

    //向字典里添加项
    public void AddTransition(Transition transition, FSMStateID id)
    {
        //检查是否存在
        if (map.ContainsKey(transition))
        {
            Debug.LogWarning("FSMState ERROR: Transition is already inside the map");
            return;
        }

        //如果不在字典中，则将转换以及转换后的状态存入字典中
        map.Add(transition, id);
        Debug.Log("Added: " + transition + "with ID: " + id);
    }

    public FSMStateID GetOutPutState(Transition transition)
    {
        return map[transition];
    }

    public void DeleteTransition(Transition transition)
    {
        //检查是否存在
        if (map.ContainsKey(transition))
        {
            map.Remove(transition);
            return;
        }

        Debug.LogWarning("FSMState ERROR: Transition passed was not on this State's List");
    }
}
