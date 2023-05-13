using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class C_Dictionary<TKey, TValue>
{
    [SerializeField]
    private List<TKey> keys;
    public List<TKey> Keys
    {
        get { return keys; }
        set { keys = value; }
    }

    [SerializeField]
    private List<TValue> values;
    public List<TValue> Values
    {
        get { return values; }
        set { values = value; }
    }

    public TValue Search(TKey key)
    {
        return values[keys.IndexOf(key)];
    }
}
