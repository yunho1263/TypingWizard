using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class D_GraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<D_GroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<D_NodeSaveData> Nodes { get; set; }
        [field: SerializeField] public List<string> OldGroupNames { get; set; }
        [field: SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
        [field: SerializeField] public List<string> OldTexts { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }


        public void Initialize(string fileName)
        {
            FileName = fileName;

            Groups = new List<D_GroupSaveData>();
            Nodes = new List<D_NodeSaveData>();
        }
    }
}
