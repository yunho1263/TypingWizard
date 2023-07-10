using UnityEngine;

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
    }
}
