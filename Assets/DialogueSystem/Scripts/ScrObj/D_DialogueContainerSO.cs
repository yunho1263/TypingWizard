using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.ScrObj
{
    public class D_DialogueContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public SerializableDictionary<D_DialogueGroupSO, List<D_DialogueSO>> DialogueGroups { get; set; }
        [field: SerializeField] public List<D_DialogueSO> UngroupedDialogues { get; set; }

        public void Initialize(string fileName)
        {
            // 필드 초기화
            FileName = fileName;
            DialogueGroups = new SerializableDictionary<D_DialogueGroupSO, List<D_DialogueSO>>();
            UngroupedDialogues = new List<D_DialogueSO>();
        }

        public List<string> GetDialogueGroupNames() // 그룹 이름 리스트 반환
        {
            // 반환할 그룹 이름 리스트
            List<string> dialogueGroupNames = new List<string>();

            // 리스트에 그룹 이름 추가
            foreach (D_DialogueGroupSO dialogueGroup in DialogueGroups.Keys)
            {
                dialogueGroupNames.Add(dialogueGroup.GroupName);
            }

            // 리스트 반환
            return dialogueGroupNames;
        }

        public List<string> GetGroupedDialogueNames(D_DialogueGroupSO dialogueGroup, bool startingDialogueOnly) // 그룹에 속한 대화 이름 리스트 반환
        {
            // 그룹에 속한 대화 리스트
            List<D_DialogueSO> groupedDialogues = DialogueGroups[dialogueGroup];

            // 반환할 그룹에 속한 대화 이름 리스트
            List<string> groupedDialogueNames = new List<string>();

            // 리스트에 그룹에 속한 대화 이름 추가
            foreach (D_DialogueSO groupedDialogue in groupedDialogues)
            {
                // 시작 대화만 반환할 경우 시작 대화가 아닌 대화는 리스트에 추가하지 않음
                if (startingDialogueOnly && !groupedDialogue.IsStartingDialogue)
                {
                    continue;
                }

                groupedDialogueNames.Add(groupedDialogue.DialogueName);
            }

            // 리스트 반환
            return groupedDialogueNames;
        }

        public List<string> GetUngroupedDialogueNames(bool startingDialogueOnly) // 그룹에 속하지 않은 대화 이름 리스트 반환
        {
            // 반환할 그룹에 속하지 않은 대화 이름 리스트
            List<string> ungroupedDialogueNames = new List<string>();

            // 리스트에 그룹에 속하지 않은 대화 이름 추가
            foreach (D_DialogueSO ungroupedDialogue in UngroupedDialogues)
            {
                // 시작 대화만 반환할 경우 시작 대화가 아닌 대화는 리스트에 추가하지 않음
                if (startingDialogueOnly && !ungroupedDialogue.IsStartingDialogue)
                {
                    continue;
                }
                ungroupedDialogueNames.Add(ungroupedDialogue.DialogueName);
            }

            // 리스트 반환
            return ungroupedDialogueNames;
        }
    }
}
