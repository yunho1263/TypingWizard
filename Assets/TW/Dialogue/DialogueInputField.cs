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
        public TMP_InputField inputField; // 입력 필드
        public string inputedStr; // 입력된 문자열

        public RectTransform inputFieldRect;
        public Vector2 sizeDelta;

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
            //Player의 스크린 좌표를 구한다
            Vector3 screenPos = Camera.main.WorldToScreenPoint(Player.instance.transform.position);
            //speechBubble의 위치를 Player의 스크린 좌표로 설정한다
            inputFieldRect.position = screenPos + new Vector3(0, 100, 0);
        }

        public void UpdateScale()
        {
            sizeDelta.x = inputField.preferredWidth + 20;
            sizeDelta.y = inputField.preferredHeight + 20;

            //현재 text길이에 맞게 inputFieldRect 크기를 조절한다
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
