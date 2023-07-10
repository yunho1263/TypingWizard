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
            // �ʵ� �ʱ�ȭ
            FileName = fileName;
            DialogueGroups = new SerializableDictionary<D_DialogueGroupSO, List<D_DialogueSO>>();
            UngroupedDialogues = new List<D_DialogueSO>();
        }

        public List<string> GetDialogueGroupNames() // �׷� �̸� ����Ʈ ��ȯ
        {
            // ��ȯ�� �׷� �̸� ����Ʈ
            List<string> dialogueGroupNames = new List<string>();

            // ����Ʈ�� �׷� �̸� �߰�
            foreach (D_DialogueGroupSO dialogueGroup in DialogueGroups.Keys)
            {
                dialogueGroupNames.Add(dialogueGroup.GroupName);
            }

            // ����Ʈ ��ȯ
            return dialogueGroupNames;
        }

        public List<string> GetGroupedDialogueNames(D_DialogueGroupSO dialogueGroup, bool startingDialogueOnly) // �׷쿡 ���� ��ȭ �̸� ����Ʈ ��ȯ
        {
            // �׷쿡 ���� ��ȭ ����Ʈ
            List<D_DialogueSO> groupedDialogues = DialogueGroups[dialogueGroup];

            // ��ȯ�� �׷쿡 ���� ��ȭ �̸� ����Ʈ
            List<string> groupedDialogueNames = new List<string>();

            // ����Ʈ�� �׷쿡 ���� ��ȭ �̸� �߰�
            foreach (D_DialogueSO groupedDialogue in groupedDialogues)
            {
                // ���� ��ȭ�� ��ȯ�� ��� ���� ��ȭ�� �ƴ� ��ȭ�� ����Ʈ�� �߰����� ����
                if (startingDialogueOnly && !groupedDialogue.IsStartingDialogue)
                {
                    continue;
                }

                groupedDialogueNames.Add(groupedDialogue.DialogueName);
            }

            // ����Ʈ ��ȯ
            return groupedDialogueNames;
        }

        public List<string> GetUngroupedDialogueNames(bool startingDialogueOnly) // �׷쿡 ������ ���� ��ȭ �̸� ����Ʈ ��ȯ
        {
            // ��ȯ�� �׷쿡 ������ ���� ��ȭ �̸� ����Ʈ
            List<string> ungroupedDialogueNames = new List<string>();

            // ����Ʈ�� �׷쿡 ������ ���� ��ȭ �̸� �߰�
            foreach (D_DialogueSO ungroupedDialogue in UngroupedDialogues)
            {
                // ���� ��ȭ�� ��ȯ�� ��� ���� ��ȭ�� �ƴ� ��ȭ�� ����Ʈ�� �߰����� ����
                if (startingDialogueOnly && !ungroupedDialogue.IsStartingDialogue)
                {
                    continue;
                }
                ungroupedDialogueNames.Add(ungroupedDialogue.DialogueName);
            }

            // ����Ʈ ��ȯ
            return ungroupedDialogueNames;
        }
    }
}
