using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TypingWizard.Spells
{
    using Enums;

    [Serializable]
    public class Spell : MonoBehaviour
    {
        public int spellID; //�ֹ� ID
        public string spellName; //�ֹ� �̸�

        public bool isAcquired; //ȹ�� ����

        [SerializeField]
        public List<Spell_Tag> spellTags; //�ֹ� �±�
        public Spell_Type spell_Type; // �ֹ� Ÿ��

        public int cost; //�ֹ� ���

        public string description; // �ֹ� ����

        public Character caster; // ������
        public List<GameObject> targets; // ���

        public SerializableDictionary<ValueType, float> values; //�ֹ� ��

        public virtual void Cast(Character caster)
        {
            Debug.Log(spellName + " " + caster.characterName);
        }

        public virtual void Initialize(Character caster)
        {
            this.caster = caster;
        }

        // ���� �ð� �� �ڽ��� ������Ʈ�� �ı��ϴ� �ڷ�ƾ
        public IEnumerator DestroySelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}