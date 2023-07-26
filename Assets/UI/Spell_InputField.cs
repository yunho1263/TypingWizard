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
        public TMP_InputField inputField; // �Է� �ʵ�

        public string inputedStr; // �Էµ� �ֹ�

        public Spell curMultipleAriasSpell; // ���� ��â���� �ֹ�
        public int doneAriasSpellSentenceIndex; // ��â�� �Ϸ�� ���� �ε���

        public Single_SpellDictionary single_SpellDictionary; // ���� ��â �ֹ� ����
        public Multiple_SpellDictionary multiple_SpellDictionary; // ���� ��â �ֹ� ����

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

            Spell castingSpell;

            // ���� ��â �ֹ� �������� �ֹ��� ã�Ƽ� ����
            if (single_SpellDictionary.Search(inputedStr, out castingSpell))
            {
                castingSpell.Cast(Player.instance);
                return;
            }

            // ���� ��â �ֹ� �������� �ֹ��� ã�Ƽ� ����
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
            // ���ڿ��� �ִ� �ܾ���� �迭�� ��ȯ�Ѵ�
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
                else // ������ �ƴ� ���
                {
                    if (isSpace) // ���� ���ڰ� �����̾��ٸ�
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
