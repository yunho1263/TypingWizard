using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TypingWizard
{
    [Serializable]
    public class ValueDictionaryEntry
    {
        public Spell.ValueType key;
        public float value;
    }

    [Serializable]
    public class Spell : MonoBehaviour
    {
        public enum ValueType
        {
            Damage,
            Heal,
            duration,
            range,
            speed,
            size,
            count
        }

        public enum Spell_Tag
        {
            AttackSpell, //������
            BuffSpell, //������
            UtilitySpell, //��ƿ��Ƽ��

            Projectile, //�߻�ü
            Range, //������
            Movement, //�̵�
            Charging, //������
        }

        public enum Araia_Type //��â Ÿ��
        {
            SpellName,
            OneSentence,
            MultipleSentence
        }

        public int spellID; //�ֹ� ID
        public string spellName; //�ֹ� �̸�

        public bool isAcquired; //ȹ�� ����
        public List<string> arias; //�ֹ� ��â

        [SerializeField]
        public List<Spell_Tag> spellTags; //�ֹ� �±�
        public Araia_Type araiaType;

        public int cost; //�ֹ� ���

        public string description; // �ֹ� ����

        public Character caster; // ������
        public List<GameObject> targets; // ���

        [SerializeField]
        public List<ValueDictionaryEntry> values; //�ֹ� ��

        public virtual void Cast(Character caster)
        {
            Debug.Log(spellName + " " + caster.characterName);
        }

        public virtual void Initialize(Character caster)
        {
            this.caster = caster;
        }
    }

}