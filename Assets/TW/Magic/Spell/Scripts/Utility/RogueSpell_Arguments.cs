using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace TypingWizard.Spells.Utility
{
    [Serializable]
    public class RogueSpell_Arguments<T>
    {
        public List<LocalizedString> keys;
        public List<T> values;

        public RogueSpell_Arguments(StringTableCollection tableCol)
        {
            keys = new List<LocalizedString>();
            values = new List<T>();

            // 테이블에 있는 모든 열거형 값을 대응하는 키값으로 설정
            foreach (T value in System.Enum.GetValues(typeof(T)))
            {
                LocalizedString newLocString = new LocalizedString();
                newLocString.TableReference = tableCol.TableCollectionNameReference;
                newLocString.TableEntryReference = "st_A: " + value.ToString();
                keys.Add(newLocString);
                values.Add(value);
                Debug.Log(newLocString.GetLocalizedString());
            }
        }

        public bool Contains(string key)
        {
            return keys.Exists(x => string.Equals(x.GetLocalizedString(), key));
        }

        public bool GetValue(string key, out T value)
        {
            int index = keys.FindIndex(x => string.Equals(x.GetLocalizedString(), key));

            if (index == -1)
            {
                value = default(T);
                return false;
            }

            value = values[index];
            return true;
        }
    }
}
