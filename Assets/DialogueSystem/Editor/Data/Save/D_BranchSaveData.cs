using System;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    [Serializable]
    public class D_BranchSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeId { get; set; }
    }
}
