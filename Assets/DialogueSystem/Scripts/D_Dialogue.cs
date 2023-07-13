using UnityEngine;
using System.Collections.Generic;

namespace DialogueSystem
{
    using ScrObj;

    public class D_Dialogue : MonoBehaviour
    {
        // 스크립터블 오브젝트
        [SerializeField] private D_DialogueContainerSO dialogueContainer;
        [SerializeField] private D_DialogueGroupSO dialogueGroup;
        [SerializeField] private D_DialogueSO dialogue;

        // 필터
        [SerializeField] private bool groupedDialogue;
        [SerializeField] private bool startingDialogueOnly;

        // 인덱스
        [SerializeField] private int selectedDialogueGroupIndex;
        [SerializeField] private int selectedDialogueIndex;

        private Dictionary<string, D_DialogueSO> startingDialogues;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (dialogueContainer == null)
            {
                return;
            }

            startingDialogues = new Dictionary<string, D_DialogueSO>();

            // 시작 대화를 찾아서 리스트에 추가
            foreach (D_DialogueGroupSO dialogueGroup in dialogueContainer.DialogueGroups.Keys)
            {
                foreach (D_DialogueSO dialogue in dialogueContainer.DialogueGroups[dialogueGroup])
                {
                    if (dialogue.IsStartingDialogue)
                    {
                        startingDialogues.Add(dialogue.DialogueName, dialogue);
                    }
                }
            }

            foreach (D_DialogueSO dialogue in dialogueContainer.UngroupedDialogues)
            {
                if (dialogue.IsStartingDialogue)
                {
                    startingDialogues.Add(dialogue.DialogueName, dialogue);
                }
            }
        }

        public D_DialogueSO GetStartingDialogue(string dialogueName)
        {
            if (startingDialogues.ContainsKey(dialogueName) == false)
            {
                return null;
            }
            return startingDialogues[dialogueName];
        }
    }
}
