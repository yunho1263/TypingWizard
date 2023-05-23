using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spell : MonoBehaviour
{
    public enum Spell_Tag
    {
        AttackSpell, //공격형
        BuffSpell, //버프형
        UtilitySpell, //유틸리티형

        Projectile, //발사체
        Range, //범위형
        Movement, //이동
        Charging, //충전형
    }

    public enum Araia_Type //영창 타입
    {
        SpellName,
        OneSentence,
        MultipleSentence
    }

    public int spellID; //주문 ID
    public string spellName; //주문 이름

    public bool isAcquired; //획득 여부
    public List<string> arias; //주문 영창

    [SerializeField]
    public List<Spell_Tag> spellTags; //주문 태그
    public Araia_Type araiaType;

    public int cost; //주문 비용

    public string description; // 주문 설명

    public GameObject caster; // 시전자
    public List<GameObject> targets; // 대상

    public void Cast(GameObject caster)
    {
        Debug.Log(spellName + " "+ caster.gameObject.name);
    }
}
