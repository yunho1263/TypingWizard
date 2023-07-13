using UnityEngine;
using System.Collections.Generic;

namespace DialogueSystem
{
    using ScrObj;

    public class D_Dialogue : MonoBehaviour
    {
        // ��ũ���ͺ� ������Ʈ
        [SerializeField] private D_DialogueContainerSO dialogueContainer;
        [SerializeField] private D_DialogueGroupSO dialogueGroup;
        [SerializeField] private D_DialogueSO dialogue;

        // ����
        [SerializeField] private bool groupedDialogue;
        [SerializeField] private bool startingDialogueOnly;

        // �ε���
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

            // ���� ��ȭ�� ã�Ƽ� ����Ʈ�� �߰�
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
