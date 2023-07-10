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
            //speaker�� ��ũ�� ��ǥ�� ���Ѵ�
            Vector3 screenPos = Camera.main.WorldToScreenPoint(speaker.transform.position);
            //speechBubble�� ��ġ�� speaker�� ��ũ�� ��ǥ�� �����Ѵ�
            speechBubble.position = screenPos;
        }

        public void UpdateScale()
        {
            sizeDelta.x = speechBubbleText.preferredWidth + 20;
            sizeDelta.y = speechBubbleText.preferredHeight + 20;
            //speechBubbleText�� ����� text�� �°� �����Ѵ�
            speechBubbleText.rectTransform.sizeDelta = sizeDelta;
            //���� speechBubbleText�� text���̿� �°� speechBubble�� ũ�⸦ �����Ѵ�
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