using System;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    [Serializable]
    public class D_BranchSaveData
    {
        [field: SerializeField] public string branchName { get; set; }
        [field: SerializeField] public string Ko_Text { get; set; }
        [field: SerializeField] public string Ja_Text { get; set; }
        [field: SerializeField] public string En_Text { get; set; }
        [field: SerializeField] public string LocalizedTextKey { get; set; }
        [field: SerializeField] public string NodeId { get; set; }
    }
}
