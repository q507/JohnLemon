using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��������AI����ʹ�õ���״̬��ת��
//ת��
public enum Transition
{
   SawPlayer = 0,   //�������
   ReachPlayer,     //�ӽ����
   LostPlayer,      //����뿪���߷�Χ
   AttackPlayer,    //������ɫ
   NoHealth,        //����
   Relax,           //��Ϣ
   Work             //��Ϣ����
}

//״̬
public enum FSMStateID
{
    patrolling = 0, //Ѳ��״̬
    Chasing,        //׷��״̬
    Attacking,      //����״̬
    Dead,           //����״̬
    Idle            //վ��״̬
}

public abstract class FSMState
{
    //���ڴ洢 ��ת��-״̬�� ��ֵ��
    protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();

    //״̬���ID
    protected FSMStateID stateID;

    public FSMStateID ID { get { return stateID; } }

    //Reson��������ȷ���Ƿ���Ҫת��������״̬��Ӧ�÷����ĸ�ת��
    public abstract void Reason();

    //Act�����������ڱ�״̬�н�ɫ����Ϊ�����ж���������
    public abstract void Act();

    //���ֵ��������
    public void AddTransition(Transition transition, FSMStateID id)
    {
        //����Ƿ����
        if (map.ContainsKey(transition))
        {
            Debug.LogWarning("FSMState ERROR: Transition is already inside the map");
            return;
        }

        //��������ֵ��У���ת���Լ�ת�����״̬�����ֵ���
        map.Add(transition, id);
        Debug.Log("Added: " + transition + "with ID: " + id);
    }

    public FSMStateID GetOutPutState(Transition transition)
    {
        return map[transition];
    }

    public void DeleteTransition(Transition transition)
    {
        //����Ƿ����
        if (map.ContainsKey(transition))
        {
            map.Remove(transition);
            return;
        }

        Debug.LogWarning("FSMState ERROR: Transition passed was not on this State's List");
    }
}
