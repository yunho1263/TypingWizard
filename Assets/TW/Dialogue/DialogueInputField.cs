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
        public TMP_InputField inputField; // 입력 필드
        public string inputedStr; // 입력된 주문
        public void OnInputValueChanged() // 입력 값이 변경될 때마다 호출
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                // 복붙 차단
                if (Input.GetKey(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                {
                    inputField.text = "";
                }
            }

            if (inputField.text.EndsWith('\n'))
            {
                inputedStr = inputField.text;
                inputedStr = inputedStr.TrimEnd('\n'); // 엔터키 제거
                inputedStr = inputedStr.TrimEnd(); // 공백 제거
                inputedStr = inputedStr.TrimStart(); // 공백 제거

                TypingFinish(); // 입력 종료
            }
        }

        public void TypingFinish()
        {
            //문자열이 없으면 종료
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
