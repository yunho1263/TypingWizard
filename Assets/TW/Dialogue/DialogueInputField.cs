using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TypingWizard.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TypingWizard
{
    public class DialogueInputField : MonoBehaviour
    {
        public TMP_InputField inputField; // �Է� �ʵ�
        public string inputedStr; // �Էµ� ���ڿ�

        public RectTransform inputFieldRect;
        public Vector2 sizeDelta;

        public void OnInputValueChanged() // �Է� ���� ����� ������ ȣ��
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                // ���� ����
                if (Input.GetKey(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                {
                    inputField.text = "";
                }
            }

            if (inputField.text.EndsWith('\n'))
            {
                inputedStr = inputField.text;
                inputedStr = inputedStr.TrimEnd('\n'); // ����Ű ����
                inputedStr = inputedStr.TrimEnd(); // ���� ����
                inputedStr = inputedStr.TrimStart(); // ���� ����

                TypingFinish(); // �Է� ����
            }
        }

        public void TypingFinish()
        {
            //���ڿ��� ������ ����
            if (inputedStr == string.Empty)
            {
                ResetField();
                return;
            }

            DialogueManager.Instance.ReceiveAnswers(inputedStr);
            ResetField();
        }

        public void ResetField()
        {
            inputField.text = "";
            inputFieldRect.gameObject.SetActive(false);
        }

        public void SetPosition()
        {
            //Player�� ��ũ�� ��ǥ�� ���Ѵ�
            Vector3 screenPos = Camera.main.WorldToScreenPoint(Player.instance.transform.position);
            //speechBubble�� ��ġ�� Player�� ��ũ�� ��ǥ�� �����Ѵ�
            inputFieldRect.position = screenPos + new Vector3(0, 100, 0);
        }

        public void UpdateScale()
        {
            sizeDelta.x = inputField.preferredWidth + 20;
            sizeDelta.y = inputField.preferredHeight + 20;

            //���� text���̿� �°� inputFieldRect ũ�⸦ �����Ѵ�
            inputFieldRect.sizeDelta = sizeDelta;
        }

        private void Update()
        {
            if (Player.instance != null)
            {
                SetPosition();
                UpdateScale();
            }
        }
    }
}
