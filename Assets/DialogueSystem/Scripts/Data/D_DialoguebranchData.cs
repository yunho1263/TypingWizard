using System;
using UnityEngine;
using UnityEngine.Localization;

namespace DialogueSystem.Data
{
    using ScrObj;
    using UnityEditor;
    using UnityEditor.Localization;
    using UnityEngine.Localization.Tables;

    [Serializable]

    public class D_DialoguebranchData
    {
        [field: SerializeField] public string BranchName { get; set; }
        [field: SerializeField] public string Ko_Text { get; set; }
        [field: SerializeField] public string Ja_Text { get; set; }
        [field: SerializeField] public string En_Text { get; set; }
        [field: SerializeField] public string LocalizedTextKey { get; set; }
        [field: SerializeField] public LocalizedString LocalizedText { get; set; }
        [field: SerializeField] public D_DialogueSO NextDialogue { get; set; }

        public void SaveLocalizedText()
        {
            var tableCol = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

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
            EditorUtility.SetDirty(ko_Table);
            EditorUtility.SetDirty(ja_Table);
            EditorUtility.SetDirty(en_Table);
        }
    }
}
