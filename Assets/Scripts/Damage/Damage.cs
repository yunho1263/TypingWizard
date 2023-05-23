using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
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
    public Character caster; // ������
    public Character target; // ������ �޴� ���
    public float damageValue; // ������ ��


    public UnityEvent<Character> OnApplyDamage; // ������ ����� �̺�Ʈ
}
