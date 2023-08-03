using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary.Utility
{
    public static class SpellDictionary_Utility
    {
        public static List<string> GetWords(string input)
        {
            // ���ڿ��� �ִ� �ܾ���� �迭�� ��ȯ�Ѵ�
            List<string> words = new List<string>();

            bool isSpace = false;
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
                        word += input[i];
                    }
                    else
                    {
                        word += input[i];
                        // ������ ���ڿ��ٸ� �ܾ �߰��ϰ� ����
                        if (i == input.Length - 1)
                        {
                            words.Add(word);
                        }
                    }
                }
            }

            return words;
        }
    }
}
