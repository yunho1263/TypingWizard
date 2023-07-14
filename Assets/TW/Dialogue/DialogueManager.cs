using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TypingWizard.Dialogue
{
    using DialogueSystem.ScrObj;
    using DialogueSystem.Enums;
    using TMPro;
    using Unity.Properties;
    using DialogueSystem.Data;
    using System;

    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager instance;
        public static DialogueManager Instance
        {
            get
            {
                return instance;
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

        // 화자의 리스트
        private Dictionary<string, DialogueSpeaker> speakers;
        private DialogueSpeaker DialogueSpeaker;

        // 현재 재생중인 대화
        private D_DialogueSO currentDialogue;

        public bool isDialoguePlaying = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            bubbleObjectPool = new List<GameObject>();
            UsingBubbleQueue = new Queue<DialogueSpeechBubble>();
            UnUsingBubbleQueue = new Queue<DialogueSpeechBubble>();
            speakers = new Dictionary<string, DialogueSpeaker>();
            bubblePrefeb = Resources.Load<GameObject>("Prefebs/DialogSpeechBubble");
            CreateBubble(5);

            GameObject inputFieldPrefeb = Resources.Load<GameObject>("Prefebs/DialogueInputField");
            dialogueInputFieldObj = Instantiate(inputFieldPrefeb, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
            dialogueInputFieldObj.SetActive(false);
            dialogueInputField = dialogueInputFieldObj.transform.GetChild(0).GetComponent<TMP_InputField>();
        }

        private void OnDestroy()
        {
            if (bubblePrefeb != null)
            {
                bubblePrefeb = null;
            }

            Resources.UnloadUnusedAssets();
            instance = null;
        }

        #region 다이얼로그 컨트롤
        public void SetDialogue(D_DialogueSO dialogue)
        {
            currentDialogue = dialogue;
        }

        public DialogueResult StartDialogue()
        {
            if(currentDialogue == null)
            {
                return DialogueResult.None;
            }

            isDialoguePlaying = true;
            Player.instance.playerInput.SwitchCurrentActionMap("Dialogue");

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
            if (currentDialogue == null)
            {
                return DialogueResult.None;
            }

            if (currentDialogue.Branchs == null || currentDialogue.Branchs.Count == 0)
            {
                currentDialogue = null;
                return DialogueResult.End;
            }

            if (currentDialogue.NodeType == DialogueNodeType.Branch)
            {
                return DialogueResult.Branch;
            }

            CloseSpeechBubble();

            currentDialogue = currentDialogue.Branchs[0].NextDialogue;

            return DialogueResult.Next;
        }

        public DialogueResult ReceiveAnswers(string answer)
        {
            foreach (D_DialoguebranchData branch in currentDialogue.Branchs)
            {
                if (string.Equals(branch.LocalizedText.GetLocalizedString(), answer))
                {
                    CloseSpeechBubble();

                    currentDialogue = branch.NextDialogue;

                    return DialogueResult.Next;
                }
            }

            currentDialogue = currentDialogue.Branchs[currentDialogue.Branchs.Count - 1].NextDialogue;
            return DialogueResult.Next;
        }

        public void EndDialogue()
        {
            isDialoguePlaying = false;
        }
        #endregion

        #region 플레이어 입력 컨트롤

        // 플레이어 입력 필드
        public GameObject dialogueInputFieldObj;
        public TMP_InputField dialogueInputField;

        public void DisplayDialogueInputField()
        {
            dialogueInputFieldObj.SetActive(true);
        }

        #endregion

        #region 말풍선 컨트롤

        // 말풍선 풀
        private GameObject bubblePrefeb;
        private List<GameObject> bubbleObjectPool;
        private Queue<DialogueSpeechBubble> UsingBubbleQueue;
        private Queue<DialogueSpeechBubble> UnUsingBubbleQueue;

        private void CreateBubble(int quantity = 1)
        {
            for (int i = 0; i < quantity; i++)
            {
                GameObject newBubbleObject = Instantiate(bubblePrefeb, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
                newBubbleObject.SetActive(false);
                bubbleObjectPool.Add(newBubbleObject);
                UnUsingBubbleQueue.Enqueue(newBubbleObject.GetComponent<DialogueSpeechBubble>());
            }
        }

        public void DisplaySpeechBubble(string speakerName, string text)
        {
            DialogueSpeaker currentSpeaker;

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

            // 화자를 가져온다
            currentSpeaker = speakers[speakerName];

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

        #region 화자 컨트롤
        public void AddSpeaker(DialogueSpeaker speaker)
        {
            if (speakers.ContainsKey(speaker.speakerName))
            {
                return;
            }

            speakers.Add(speaker.speakerName, speaker);
        }

        public void RemoveSpeaker(DialogueSpeaker speaker)
        {
            if (speakers.ContainsKey(speaker.speakerName) == false)
            {
                return;
            }

            speakers.Remove(speaker.speakerName);
        }
        #endregion
    }
}
