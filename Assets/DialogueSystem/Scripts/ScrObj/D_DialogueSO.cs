using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEditor.Localization;

namespace DialogueSystem.ScrObj
{
    using Data;
    using Enums;
    using UnityEditor;
    using UnityEditor.Rendering;
    using UnityEngine.InputSystem;
    using UnityEngine.Localization.Settings;
    using UnityEngine.Localization.Tables;

    public class D_DialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] public string GroupName { get; set; }
        [field: SerializeField] [field: TextArea()] public string Speaker { get; set; }
        [field: SerializeField] [field: TextArea()] public string Ko_Text { get; set; }
        [field: SerializeField] [field: TextArea()] public string Ja_Text { get; set; }
        [field: SerializeField] [field: TextArea()] public string En_Text { get; set; }
        [field: SerializeField] public string LocalizedTextKey { get; set; }
        [field: SerializeField] public LocalizedString LocalizedText { get; set; }
        [field: SerializeField] public List<D_DialoguebranchData> Branchs { get; set; }
        [field: SerializeField] public DialogueNodeType NodeType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, string groupName, string speaker, string ko_text, string ja_text, string en_text, List<D_DialoguebranchData> branch, DialogueNodeType nodeType, bool isStart, string localizedTextKey)
        {
            DialogueName = dialogueName;
            GroupName = groupName;
            Speaker = speaker;
            Ko_Text = ko_text;
            Ja_Text = ja_text;
            En_Text = en_text;
            Branchs = branch;
            NodeType = nodeType;
            IsStartingDialogue = isStart;
            LocalizedTextKey = localizedTextKey;
        }

        public void SaveLocalizedText()
        {
            StringTableCollection tableCol = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            StringTable ko_Table = tableCol.GetTable("ko") as StringTable;
            StringTable ja_Table = tableCol.GetTable("ja") as StringTable;
            StringTable en_Table = tableCol.GetTable("en") as StringTable;

            ko_Table.AddEntry(LocalizedTextKey, Ko_Text);
            ja_Table.AddEntry(LocalizedTextKey, Ja_Text);
            en_Table.AddEntry(LocalizedTextKey, En_Text);

            LocalizedText = new LocalizedString();
            LocalizedText.TableReference = tableCol.TableCollectionNameReference.TableCollectionName;
            LocalizedText.TableEntryReference = LocalizedTextKey;

            EditorUtility.SetDirty(tableCol);
            EditorUtility.SetDirty(tableCol.SharedData);
        }
    }
}
