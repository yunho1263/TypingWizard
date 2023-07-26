using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TypingWizard.UI
{
    using Spells;
    using SpellDictionary;

    public class Spell_InputField : MonoBehaviour
    {
        public TMP_InputField inputField; // 입력 필드

        public string inputedStr; // 입력된 주문

        public Spell curMultipleAriasSpell; // 현재 영창중인 주문
        public int doneAriasSpellSentenceIndex; // 영창이 완료된 문장 인덱스

        public Single_SpellDictionary single_SpellDictionary; // 단일 영창 주문 사전
        public Multiple_SpellDictionary multiple_SpellDictionary; // 다중 영창 주문 사전

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
            ResetField();
            //문자열이 없으면 종료
            if (inputedStr == string.Empty)
            {
                return;
            }

            Spell castingSpell;

            // 단일 영창 주문 사전에서 주문을 찾아서 시전
            if (single_SpellDictionary.Search(inputedStr, out castingSpell))
            {
                castingSpell.Cast(Player.instance);
                return;
            }

            // 다중 영창 주문 사전에서 주문을 찾아서 시전
            if (multiple_SpellDictionary.Search(inputedStr, out castingSpell))
            {
                castingSpell.Cast(Player.instance);
                return;
            }
        }

        public void ResetField()
        {
            Player.instance.playerInput.currentActionMap = Player.instance.playerInput.actions.FindActionMap("Player");
            inputField.text = "";
            inputField.gameObject.SetActive(false);
        }

        public List<string> GetWords(string input)
        {
            // 문자열에 있는 단어들을 배열로 반환한다
            List<string> words = new List<string>();

            bool isSpace = true;
            string word = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    if (!isSpace)
                    {
                        isSpace = true;
                        words.Add(word);
                        word = "";
                    }
                }
                else // 공백이 아닐 경우
                {
                    if (isSpace) // 이전 문자가 공백이었다면
                    {
                        isSpace = false;
                    }
                    else
                    {
                        word += input[i];
                    }
                }
            }

            return words;
        }
    }
}
