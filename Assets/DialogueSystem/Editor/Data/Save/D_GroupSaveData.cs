using System;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    [Serializable]
    public class D_GroupSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}
