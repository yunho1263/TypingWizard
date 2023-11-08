using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TypingWizard.Damages
{
    using Spells;
    using TypingWizard.Spells.Enums;

    [Serializable]
    public class Damage
    {
        public enum DamageType
        {
            Physical,
            Magical,
            Pure
        }
        public DamageType damageType;
        public enum DamageCategori
        {
            Normal,
            Persistence
        }
        public DamageCategori damageCategori;
        public enum DamageElement
        {
            None,
            Fire,
            Water,
            Cold,
            Thunder,
            Earth,
            Wind,
            Light,
            Dark,


            Rogue_Elemental_None = Rogue_Elemental.None,
            Rogue_Elemental_Fire = Rogue_Elemental.Fire,
            Rogue_Elemental_Water = Rogue_Elemental.Water,
            Rogue_Elemental_Cold = Rogue_Elemental.Cold,
            Rogue_Elemental_Thunder = Rogue_Elemental.Thunder,
            Rogue_Elemental_Earth = Rogue_Elemental.Earth,
            Rogue_Elemental_Wind = Rogue_Elemental.Wind,
            Rogue_Elemental_Light = Rogue_Elemental.Light,
            Rogue_Elemental_Dark = Rogue_Elemental.Dark

        }
        public DamageElement damageElement;

        public string damageName; // 이름
        public string Description; // 설명
        public Character owner; // 시전자
        public Character target; // 영향을 받는 대상
        public float damageValue; // 데미지 값


        public UnityEvent<Character> OnApplyDamage; // 데미지 적용시 이벤트

        public Damage(DamageScrObj d_ScrObj)
        {
            damageType = d_ScrObj.D_Type;
            damageCategori = d_ScrObj.D_Categori;
            damageElement = d_ScrObj.D_Element;
            damageName = d_ScrObj.D_Name;
            Description = d_ScrObj.D_Description;
        }

        public void Initialize(Spell spell, Damage damage)
        {
            owner = spell.caster;

            damageType = damage.damageType;
            damageCategori = damage.damageCategori;
            damageElement = damage.damageElement;
            damageName = damage.damageName;
            Description = damage.Description;
            damageValue = damage.damageValue;
        }
    }

}
