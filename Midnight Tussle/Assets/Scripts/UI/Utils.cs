using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static T RandomFromList<T>(List<T> list){
        if(list.Count == 0){
            return default(T);
        }
        return list[Random.Range(0, list.Count)];
    } 
}

public class Reference<T>
{
    public T Value { get; set; }
    public Reference() : this(default(T))
    {}
    public Reference(T val)
    {
        Value = val;
    }

    public override string ToString()
    {
        return this.Value.ToString();
    }
}