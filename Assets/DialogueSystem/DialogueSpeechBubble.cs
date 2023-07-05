using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueSpeechBubble : MonoBehaviour
    {
        public GameObject speaker;
        public RectTransform speechBubble;
        public TMP_Text speechBubbleText;
        public string text;

        public Vector2 sizeDelta;

        public void ShowSpeechBubble()
        {
            speechBubble.gameObject.SetActive(true);
        }

        public void HideSpeechBubble()
        {
            speechBubble.gameObject.SetActive(false);
        }


        public void SetDialog(GameObject speaker, string text)
        {
            this.speaker = speaker;
            this.text = text;
            speechBubbleText.text = this.text;
        }

        public void SetPosition()
        {
            //speaker의 스크린 좌표를 구한다
            Vector3 screenPos = Camera.main.WorldToScreenPoint(speaker.transform.position);
            //speechBubble의 위치를 speaker의 스크린 좌표로 설정한다
            speechBubble.position = screenPos;
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
            //HideSpeechBubble();
        }

        private void Update()
        {
            if (speaker != null)
            {
                SetPosition();
                UpdateScale();
            }
        }
    }

}