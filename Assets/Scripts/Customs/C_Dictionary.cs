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
        for (int i = 0; i < keys.Count; i++) // 모든 키를 찾아본다
        {
            if (keys[i].Equals(key)) // 키를 찾았다면
            {
                return values[i];
            }
        }
        // 키를 찾을 수 없었다면
        return default(TValue);
    }
}
