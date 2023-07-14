using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TypingWizard
{
    public class DialogueInputField : MonoBehaviour
    {
        public TMP_InputField inputField; // �Է� �ʵ�
        public string inputedStr; // �Էµ� �ֹ�
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
        }

        public void ResetField()
        {
            Player.instance.playerInput.currentActionMap = Player.instance.playerInput.actions.FindActionMap("Dialogue");
            inputField.gameObject.SetActive(false);
            inputField.text = "";
        }
    }
}
