using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary.Utility
{
    public static class SpellDictionary_Utility
    {
        public static List<string> GetWords(string input)
        {
            // 문자열에 있는 단어들을 배열로 반환한다
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
                else // 공백이 아닐 경우
                {
                    if (isSpace) // 이전 문자가 공백이었다면
                    {
                        isSpace = false;
                        word += input[i];
                    }
                    else
                    {
                        word += input[i];
                        // 마지막 문자였다면 단어를 추가하고 종료
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
