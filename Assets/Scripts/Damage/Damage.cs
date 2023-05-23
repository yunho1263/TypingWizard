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

    public string damageName; // 이름
    public string Description; // 설명
    public Character caster; // 시전자
    public Character target; // 영향을 받는 대상
    public float damageValue; // 데미지 값


    public UnityEvent<Character> OnApplyDamage; // 데미지 적용시 이벤트
}
