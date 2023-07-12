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

        public string healName; // �̸�
        public string description; // ����
        public GameObject caster; // ������
        public GameObject target; // ������ �޴� ���
        public float value; // ȸ�� ��

        public UnityEvent<Character> OnApplyHeal; // ȸ�� ����� �̺�Ʈ
    }

}