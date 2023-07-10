using UnityEngine;
using System.Collections.Generic;

namespace DialogueSystem.ScrObj
{
    using Data;
    using Enums;

    public class D_DialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] [field:TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<D_DialoguebranchData> Branchs { get; set; }
        [field: SerializeField] public DialogueNodeType NodeType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, string text, List<D_DialoguebranchData> branch, DialogueNodeType nodeType, bool isStart)
        {
            DialogueName = dialogueName;
            Text = text;
            Branchs = branch;
            NodeType = nodeType;
            IsStartingDialogue = isStart;
        }
    }
}
