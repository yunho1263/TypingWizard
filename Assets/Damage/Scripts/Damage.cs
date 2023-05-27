using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        Earth,
        Wind,
        Light,
        Dark
    }
    public DamageElement damageElement;

    public string damageName; // �̸�
    public string Description; // ����
    public Character owner; // ������
    public Character target; // ������ �޴� ���
    public float damageValue; // ������ ��


    public UnityEvent<Character> OnApplyDamage; // ������ ����� �̺�Ʈ

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
