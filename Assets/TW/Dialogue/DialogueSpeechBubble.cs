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
            //speaker�� ��ũ�� ��ǥ�� ���Ѵ�
            Vector3 screenPos = Camera.main.WorldToScreenPoint(speakerObject.transform.position);
            //speechBubble�� ��ġ�� speaker�� ��ũ�� ��ǥ�� �����Ѵ�
            speechBubble.position = screenPos + new Vector3(0, 100, 0);
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