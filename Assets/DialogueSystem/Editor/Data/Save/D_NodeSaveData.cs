using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    using Enums;

    [Serializable]
    public class D_NodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public List<D_BranchSaveData> Branchs { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public DialogueNodeType DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}
