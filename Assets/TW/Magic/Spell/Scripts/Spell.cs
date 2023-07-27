using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TypingWizard.Spells
{
    using Enums;
    [Serializable]
    public class SpellValueDictionaryEntry
    {
        public ValueType key;
        public float value;
    }

    [Serializable]
    public class Spell : MonoBehaviour
    {
        public int spellID; //주문 ID
        public string spellName; //주문 이름

        public bool isAcquired; //획득 여부

        [SerializeField]
        public List<Spell_Tag> spellTags; //주문 태그
        public Spell_Type spell_Type; // 주문 타입

        public int cost; //주문 비용

        public string description; // 주문 설명

        public Character caster; // 시전자
        public List<GameObject> targets; // 대상

        [SerializeField]
        public List<SpellValueDictionaryEntry> values; //주문 값

        public virtual void Cast(Character caster)
        {
            //Debug.Log(spellName + " " + caster.characterName);
        }

        public virtual void Initialize(Character caster)
        {
            this.caster = caster;
        }
    }

}