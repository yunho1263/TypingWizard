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

    public enum Value_Type
    {
        Damage, //데미지
        Heal, //회복량
        Effect, //효과
        Duration, //지속시간
        Range, //범위
        Speed, //속도
    }

    public bool isAcquired;

    public string spellName;

    [SerializeField]
    public List<Spell_Tag> spellTags; //주문 태그

    public int cost; //주문 비용

    public C_Dictionary<Value_Type, float> values; //주문 값

    public string description; // 주문 설명
    public void Cast(GameObject caster)
    {
        Debug.Log(spellName + " "+ caster.gameObject.name);
    }
}
