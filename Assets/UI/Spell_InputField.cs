using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TypingWizard.UI
{
    public class Spell_InputField : MonoBehaviour
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
            ResetField();
            //���ڿ��� ������ ����
            if (inputedStr == string.Empty)
            {
                return;
            }

            Player.instance.Search(inputedStr);
        }

        public void ResetField()
        {
            Player.instance.playerInput.currentActionMap = Player.instance.playerInput.actions.FindActionMap("Player");
            inputField.text = "";
            inputField.gameObject.SetActive(false);
        }
    }
}
