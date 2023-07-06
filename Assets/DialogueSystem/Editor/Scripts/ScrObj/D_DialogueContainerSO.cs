using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.ScrObj
{
    public class D_DialogueContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public SerializableDictionary<D_DialogueGroupSO, List<D_DialogueSO>> DialogueGroups { get; set; }
        [field: SerializeField] public List<D_DialogueSO> ungroupedDialogues { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            DialogueGroups = new SerializableDictionary<D_DialogueGroupSO, List<D_DialogueSO>>();
            ungroupedDialogues = new List<D_DialogueSO>();
        }
    }
}
