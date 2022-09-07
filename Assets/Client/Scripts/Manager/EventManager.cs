using System;
using System.Collections;
using System.Collections.Generic;

public class EventManager
{
    //ֻ����������ֵ���
    public interface IRegisterations
    {

    }

    //���ע��
    public class Registerations<T> : IRegisterations
    {
        //ί��һ�Զ��ע��
        public Action<T> OnReceives = obj => { };
    }

    //�¼��ֵ�
    private static Dictionary<Type, IRegisterations> typeEventDic = new Dictionary<Type, IRegisterations>();

    //ע���¼�
    public static void Register<T>(Action<T> OnReceive)
    {
        var type = typeof(T);
        IRegisterations registerations = null;
        if(typeEventDic.TryGetValue(type, out registerations))
        {
            var register = registerations as Registerations<T>;
            register.OnReceives += OnReceive;
        }
        else
        {
            var register = new Registerations<T>();
            register.OnReceives += OnReceive;
            typeEventDic.Add(type, register);
        }
    }

    //ע���¼�
    public static void UnRegister<T>(Action<T> OnReceive)
    {
        var type = typeof(T);
        IRegisterations registerations = null;
        if (typeEventDic.TryGetValue(type, out registerations))
        {
            var register = registerations as Registerations<T>;
            register.OnReceives -= OnReceive;
        }
    }

    //�����¼�
    public static void Send<T>(T t)
    {
        var type = typeof(T);
        IRegisterations registerations = null;
        if (typeEventDic.TryGetValue(type, out registerations))
        {
            var register = registerations as Registerations<T>;
            register.OnReceives(t);
        }
    }
}
