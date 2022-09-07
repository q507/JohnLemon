using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFSN : FSM
{
    //FSM�з�����״̬�����FSMState����ɵ��б�
    private List<FSMState> fsmStates;

    public int FSMStateLength { get { return fsmStates.Count; } }

    //��ǰ״̬�ı��ID
    private FSMStateID currentStateID;
    public FSMStateID CurrentStateID { get { return currentStateID; } }

    //��ǰ״̬
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public AdvancedFSN()
    {
        fsmStates = new List<FSMState>();
    }

    //��״̬�б������һ����״̬
    public void AddFSMState(FSMState fsmState)
    {
        if(fsmStates == null)
        {
            Debug.LogError("FSM ERROR: Null referance is not allowed");
        }

        //����������״̬ʱ���б�Ϊ�գ���������б�
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

        //��������������
        fsmStates.Add(fsmState);
    }

    public void DeleteState(FSMStateID fsmState)
    {
        //��������״̬�б����Ҫɾ����״̬���б��У���ɾ��
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

    //���ݵ�ǰ״̬���Ͳ����д��ݵ�ת����ת�Ƶ���״̬
    public void PerformTransition(Transition transition)
    {
        //���ݵ�ǰ��״̬�࣬��transitionΪ������������GetOutPutState����
        FSMStateID id = currentState.GetOutPutState(transition);
        //���õ�ǰ״̬���Ϊ��״̬���
        currentStateID = id;
        //���õ�ǰ״̬Ϊ��״̬
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
