using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TypingWizard.UI
{
    public class Spell_InputField : MonoBehaviour
    {
        public TMP_InputField inputField; // 입력 필드

        public string inputedStr; // 입력된 주문

        public Spell curMultipleAriasSpell; // 현재 영창중인 주문
        public int doneAriasSpellSentenceIndex; // 영창이 완료된 문장 인덱스

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

            if (curMultipleAriasSpell != null)
            {
                if (inputedStr.CompareTo(curMultipleAriasSpell.arias[doneAriasSpellSentenceIndex + 1]) == 0) // 영창이 완료된 문장이 다음 문장이면
                {
                    if (curMultipleAriasSpell.arias.Count == doneAriasSpellSentenceIndex + 2) // 마지막 문장이면
                    {
                        curMultipleAriasSpell.Cast(Player.instance); // 주문 시전
                        curMultipleAriasSpell = null;
                        ResetField();
                        return;
                    }
                    else
                    {
                        doneAriasSpellSentenceIndex++; // 다음 문장으로
                        ResetField();
                        return;
                    }
                }
                else
                {
                    curMultipleAriasSpell = null; // 영창실패
                    doneAriasSpellSentenceIndex = 0;
                }
            }

            GameObject castingSpellObj = Player.instance.spell_BinaryTree.Search(inputedStr);
            if (castingSpellObj == null)
            {
                Debug.Log("주문이 존재하지 않습니다.");
                ResetField();
                return;
            }

            castingSpellObj.TryGetComponent(out Spell spell); // 주문 컴포넌트 가져오기
            Spell.Araia_Type araia_Type = spell.araiaType; // 주문의 영창 타입 가져오기

            switch (araia_Type)
            {
                case Spell.Araia_Type.SpellName:
                case Spell.Araia_Type.OneSentence:
                    spell.Cast(Player.instance);
                    curMultipleAriasSpell = null;
                    break;
                case Spell.Araia_Type.MultipleSentence:
                    curMultipleAriasSpell = spell;
                    doneAriasSpellSentenceIndex = 0;
                    break;
                default:
                    break;
            }

            ResetField();
        }

        public void ResetField()
        {
            Player.instance.playerInput.currentActionMap = Player.instance.playerInput.actions.FindActionMap("Player");
            inputField.text = "";
            inputField.gameObject.SetActive(false);
        }
    }

}
