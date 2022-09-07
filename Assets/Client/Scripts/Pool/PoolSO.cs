using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolSO<T> : ScriptableObject, IPool<T>
{
    protected readonly Stack<T> Available = new Stack<T>();

    public IFactory<T> Factory { get; set; }

    private bool HasBeenPrewarmed { get; set; }

    public virtual T Create()
    {
        return Factory.Create();
    }

    public virtual void Prewarmed(int num)
    {
        if(HasBeenPrewarmed)
        {
            return;
        }
        for(int i = 0; i < num; i++)
        {
            Available.Push(Create());
        }
    }

    public virtual T Request()
    {
        return Available.Count > 0 ? Available.Pop() : Create();
    }

    public virtual IEnumerable<T> Request(int num = 1)
    {
        List<T> members = new List<T>(num);
        for(int i = 0; i < num; i++)
        {
            members.Add(Request());
        }
        return members;
    }

    public virtual void Return(T member)
    {
        Available.Push(member);
    }

    public virtual void Return(IEnumerable<T> members)
    {
        foreach(T member in members)
        {
            Return(member);
        }
    }

    public virtual void OnDisable()
    {
        Available.Clear();
        HasBeenPrewarmed = false;
    }
}
