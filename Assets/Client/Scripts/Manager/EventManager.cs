using System;
using System.Collections;
using System.Collections.Generic;

public class EventManager
{
    //只负责存在在字典中
    public interface IRegisterations
    {

    }

    //多个注册
    public class Registerations<T> : IRegisterations
    {
        //委托一对多的注册
        public Action<T> OnReceives = obj => { };
    }

    //事件字典
    private static Dictionary<Type, IRegisterations> typeEventDic = new Dictionary<Type, IRegisterations>();

    //注册事件
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

    //注销事件
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

    //发送事件
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
