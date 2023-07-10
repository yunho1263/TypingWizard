using System;
using UnityEngine;

namespace DialogueSystem.Data
{
    using ScrObj;
    [Serializable]

    public class D_DialoguebranchData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public D_DialogueSO NextDialogue { get; set; }
    }
}
