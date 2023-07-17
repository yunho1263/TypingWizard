using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TypingWizard.Dialogue
{
    public class DialogueSpeechBubble : MonoBehaviour
    {
        public GameObject speakerObject;
        public DialogueSpeaker speaker;
        public RectTransform speechBubble;
        public TMP_Text speechBubbleText;
        public TypewriterByCharacter typewriter;
        public string text;

        public Vector2 sizeDelta;

        public void Display(DialogueSpeaker speaker, string text)
        {
            speechBubble.gameObject.SetActive(true);
            SetDialog(speaker, text);
        }

        public void Close()
        {
            speechBubble.gameObject.SetActive(false);
            speakerObject = null;
            text = null;
            speechBubbleText.text = null;
        }


        public void SetDialog(DialogueSpeaker speaker, string text)
        {
            this.speaker = speaker;
            speakerObject = speaker.gameObject;
            this.text = text;
            typewriter.ShowText(text);
        }

        public void SetPosition()
        {
            //speaker의 스크린 좌표를 구한다
            Vector3 screenPos = Camera.main.WorldToScreenPoint(speakerObject.transform.position);
            //speechBubble의 위치를 speaker의 스크린 좌표로 설정한다
            speechBubble.position = screenPos + new Vector3(0, 100, 0);
        }

        public void UpdateScale()
        {
            sizeDelta.x = speechBubbleText.preferredWidth + 20;
            sizeDelta.y = speechBubbleText.preferredHeight + 20;
            //speechBubbleText의 사이즈를 text에 맞게 조절한다
            speechBubbleText.rectTransform.sizeDelta = sizeDelta;
            //현재 speechBubbleText의 text길이에 맞게 speechBubble의 크기를 조절한다
            speechBubble.sizeDelta = sizeDelta;
        }

        private void Start()
        {
            speechBubble = GetComponent<RectTransform>();
            speechBubbleText = GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            if (speakerObject != null)
            {
                SetPosition();
                UpdateScale();
            }
        }
    }
}