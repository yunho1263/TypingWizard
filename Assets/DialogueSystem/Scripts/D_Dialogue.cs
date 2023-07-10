using UnityEngine;

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
    }
}
