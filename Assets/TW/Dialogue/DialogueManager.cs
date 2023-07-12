using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TypingWizard.Dialogue
{
    using DialogueSystem;
    using DialogueSystem.ScrObj;
    using DialogueSystem.Enums;

    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager instance;
        public static DialogueManager Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else return instance = new DialogueManager();
            }
        }

        public enum DialogueResult
        {
            Start,
            Next,
            End,
            Branch,
            None
        }

        // 말풍선 풀
        private List<GameObject> bubbleObjectPool;
        private Queue<DialogueSpeechBubble> UsingBubbleQueue;
        private Queue<DialogueSpeechBubble> UnUsingBubbleQueue;

        // 화자의 리스트
        private Dictionary<string, GameObject> speakers;

        // 현재 재생중인 대화
        private D_Dialogue currentDialogueContainer;
        private D_DialogueSO currentDialogue;

        bool isDialoguePlaying = false;

        private DialogueManager()
        {
            bubbleObjectPool = new List<GameObject>();
            UsingBubbleQueue = new Queue<DialogueSpeechBubble>();
            UnUsingBubbleQueue = new Queue<DialogueSpeechBubble>();
            speakers = new Dictionary<string, GameObject>();

            CreateBubble(5);
        }

        #region 다이얼로그 컨트롤
        public void SetDialogueContainer(D_Dialogue dialogue)
        {
            currentDialogueContainer = dialogue;
        }

        public DialogueResult StartDialogue(string DialogueName)
        {
            if((currentDialogue = currentDialogueContainer.GetStartingDialogue(DialogueName)) == null)
            {
                return DialogueResult.None;
            }

            isDialoguePlaying = true;

            DisplayDialogue();
            return DialogueResult.Start;
        }

        public void DisplayDialogue()
        {
            if(currentDialogue != null)
            {
                DisplaySpeechBubble(currentDialogue.Speaker, currentDialogue.LocalizedText.GetLocalizedString());
            }
        }

        public DialogueResult NextDialogue()
        {
            CloseSpeechBubble();

            if (currentDialogue == null)
            {
                return DialogueResult.None;
            }

            if (currentDialogue.Branchs == null || currentDialogue.Branchs.Count == 0)
            {
                return DialogueResult.End;
            }

            if (currentDialogue.NodeType == DialogueNodeType.Branch)
            {
                return DialogueResult.Branch;
            }

            D_DialogueSO nextDialogue = currentDialogue.Branchs[0].NextDialogue;

            currentDialogue = nextDialogue;

            return DialogueResult.Next;
        }

        public void EndDialogue()
        {
            isDialoguePlaying = false;
        }
        #endregion

        #region 말풍선 컨트롤

        private void CreateBubble(int quantity = 1)
        {
            for (int i = 0; i < quantity; i++)
            {
                GameObject speechBubblePrefeb = Resources.Load<GameObject>("TW/Dialogue/Prefebs/DialogSpeechBubble");
                GameObject newBubbleObject = Instantiate(speechBubblePrefeb);
                bubbleObjectPool.Add(newBubbleObject);
                UnUsingBubbleQueue.Enqueue(newBubbleObject.GetComponent<DialogueSpeechBubble>());
            }
        }

        public void DisplaySpeechBubble(string speakerName, string text)
        {
            GameObject currentSpeaker;

            // 말풍선이 없으면 생성
            if (UnUsingBubbleQueue.Count == 0)
            {
                CreateBubble();
            }

            // 화자가 없으면 오류
            if (speakerName == null)
            {
                return;
            }

            if (speakers.ContainsKey(speakerName) == false)
            {
                return;
            }
            else
            {
                // 화자를 가져온다
                currentSpeaker = speakers[speakerName];
            }

            DialogueSpeechBubble bubble = UnUsingBubbleQueue.Dequeue();
            UsingBubbleQueue.Enqueue(bubble);

            bubble.Display(currentSpeaker, text);
        }

        public void CloseSpeechBubble()
        {
            // 말풍선이 없으면 스킵
            if (UsingBubbleQueue.Count == 0)
            {
                return;
            }

            DialogueSpeechBubble bubble = UsingBubbleQueue.Dequeue();
            UnUsingBubbleQueue.Enqueue(bubble);

            bubble.Close();
        }

        #endregion
    }
}
