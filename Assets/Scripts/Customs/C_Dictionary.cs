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
        for (int i = 0; i < keys.Count; i++) // ��� Ű�� ã�ƺ���
        {
            if (keys[i].Equals(key)) // Ű�� ã�Ҵٸ�
            {
                return values[i];
            }
        }
        // Ű�� ã�� �� �����ٸ�
        return default(TValue);
    }
}
