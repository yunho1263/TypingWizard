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

        public Spell curMultipleAriasSpell; // ���� ��â���� �ֹ�
        public int doneAriasSpellSentenceIndex; // ��â�� �Ϸ�� ���� �ε���

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

            if (curMultipleAriasSpell != null)
            {
                if (inputedStr.CompareTo(curMultipleAriasSpell.arias[doneAriasSpellSentenceIndex + 1]) == 0) // ��â�� �Ϸ�� ������ ���� �����̸�
                {
                    if (curMultipleAriasSpell.arias.Count == doneAriasSpellSentenceIndex + 2) // ������ �����̸�
                    {
                        curMultipleAriasSpell.Cast(Player.instance); // �ֹ� ����
                        curMultipleAriasSpell = null;
                        ResetField();
                        return;
                    }
                    else
                    {
                        doneAriasSpellSentenceIndex++; // ���� ��������
                        ResetField();
                        return;
                    }
                }
                else
                {
                    curMultipleAriasSpell = null; // ��â����
                    doneAriasSpellSentenceIndex = 0;
                }
            }

            GameObject castingSpellObj = Player.instance.spell_BinaryTree.Search(inputedStr);
            if (castingSpellObj == null)
            {
                Debug.Log("�ֹ��� �������� �ʽ��ϴ�.");
                ResetField();
                return;
            }

            castingSpellObj.TryGetComponent(out Spell spell); // �ֹ� ������Ʈ ��������
            Spell.Araia_Type araia_Type = spell.araiaType; // �ֹ��� ��â Ÿ�� ��������

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
