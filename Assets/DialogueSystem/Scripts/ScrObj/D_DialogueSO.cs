using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEditor;
using UnityEditor.Localization;

namespace DialogueSystem.ScrObj
{
    using Data;
    using Enums;
    

    public class D_DialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] [field: TextArea()] public string Speaker { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public LocalizedString LocalizedText { get; set; }
        [field: SerializeField] public List<D_DialoguebranchData> Branchs { get; set; }
        [field: SerializeField] public DialogueNodeType NodeType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, string speaker, string text, List<D_DialoguebranchData> branch, DialogueNodeType nodeType, bool isStart)
        {
            DialogueName = dialogueName;
            Speaker = speaker;
            Text = text;
            Branchs = branch;
            NodeType = nodeType;
            IsStartingDialogue = isStart;

            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            LocalizedText = new LocalizedString();
            LocalizedText.TableReference = table.TableCollectionNameReference.TableCollectionName;
            LocalizedText.TableEntryReference = "st_D: " + Text;
        }
    }
}
