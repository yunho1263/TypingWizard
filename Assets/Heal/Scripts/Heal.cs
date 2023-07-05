using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TypingWizard
{
    public class Heal : MonoBehaviour
    {
        public enum HealType
        {
            Physical,
            Magical,
            Pure
        }
        public HealType healType;
        public enum HealCategori
        {
            Normal,
            Persistence
        }
        public HealCategori healCategori;

        public string healName; // 이름
        public string description; // 설명
        public GameObject caster; // 시전자
        public GameObject target; // 영향을 받는 대상
        public float value; // 회복 값

        public UnityEvent<Character> OnApplyHeal; // 회복 적용시 이벤트
    }

}