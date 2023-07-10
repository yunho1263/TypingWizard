using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Utilities
{
    public static class CollectionUtility
    {
        public static void AddItem<K, V>(this SerializableDictionary<K, List<V>> serializableDictionary, K key, V value)
        {
            // 딕셔너리에 키가 존재하면 해당 키에 값을 추가하고, 존재하지 않으면 새로운 키와 값을 추가한다.
            if (serializableDictionary.ContainsKey(key))
            {
                serializableDictionary[key].Add(value);

                return;
            }

            serializableDictionary.Add (key, new List<V>() { value });
        }
    }
}
