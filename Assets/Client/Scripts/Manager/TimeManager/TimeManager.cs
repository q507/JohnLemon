using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    private static TimeManager instance;
    public static TimeManager Instance()
    {
        if (instance == null)
        {
            instance = new TimeManager();
        }
        return instance;
    }

    public delegate void Interval();
    private Dictionary<Interval, float> timeDic = new Dictionary<Interval, float>();

    public void AddInterval(Interval interval, float time)
    {
        if(interval != null)
        {
            timeDic[interval] = Time.time + time;
        }
    }

    public void RemoveInterval(Interval interval)
    {
        if(interval != null)
        {
            if (timeDic.ContainsKey(interval))
            {
                timeDic.Remove(interval);
            }
        }
    }

    public void Update()
    {
        if(timeDic.Count > 0)
        {
            List<Interval> remove = new List<Interval>();
            foreach(KeyValuePair<Interval, float> keyValue in timeDic)
            {
                if (keyValue.Value <= Time.time)
                {
                    remove.Add(keyValue.Key);
                }
            }
            for (int i = 0; i < remove.Count; i++)
            {
                remove[i]();
                timeDic.Remove(remove[i]);
            }
        }
    }
}
