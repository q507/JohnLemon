using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFSN : FSM
{
    //FSM中发所有状态（多个FSMState）组成的列表
    private List<FSMState> fsmStates;

    public int FSMStateLength { get { return fsmStates.Count; } }

    //当前状态的编号ID
    private FSMStateID currentStateID;
    public FSMStateID CurrentStateID { get { return currentStateID; } }

    //当前状态
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public AdvancedFSN()
    {
        fsmStates = new List<FSMState>();
    }

    //向状态列表中添加一个新状态
    public void AddFSMState(FSMState fsmState)
    {
        if(fsmStates == null)
        {
            Debug.LogError("FSM ERROR: Null referance is not allowed");
        }

        //如果插入这个状态时，列表为空，则将其加入列表
        if(fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.ID;
            return;
        }

        foreach (FSMState state in fsmStates)
        {
            if(state.ID == fsmState.ID)
            {
                Debug.LogError("FSM ERROR: Trying to add a state that was already inside the list: " + fsmState.ID.ToString());
                return;
            }
        }

        //如果不存在则插入
        fsmStates.Add(fsmState);
    }

    public void DeleteState(FSMStateID fsmState)
    {
        //搜索整个状态列表，如果要删除的状态在列表中，则删除
        for (int i = 0; i < fsmStates.Count; i++)
        {
            if (fsmStates[i].ID == fsmState)
            {
                fsmStates.Remove(fsmStates[i]);
                return;
            }
        }

        Debug.LogError("FSM ERROR: The state passed was not on the list");
    }

    //根据当前状态，和参数中传递的转换，转移到新状态
    public void PerformTransition(Transition transition)
    {
        //根据当前的状态类，以transition为参数调用他的GetOutPutState方法
        FSMStateID id = currentState.GetOutPutState(transition);
        //设置当前状态编号为新状态编号
        currentStateID = id;
        //设置当前状态为新状态
        foreach (FSMState state in fsmStates)
        {
            if(state.ID == CurrentStateID)
            {
                currentState = state;
                break;
            }
        }
    }
}
