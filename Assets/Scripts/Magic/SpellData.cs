using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum Language
{
    English,
    Korean
}

public enum Spell_Tag
{
    AttackSpell,
    BuffSpell,
    UtilitySpell,

    Projectile,
    Range,
    Movement
}

public enum Value_Type
{
    Damage,
    Heal,
    Effect,
    Duration,
    Range,
    Speed
}

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObject/SpellData", order = int.MaxValue)]
public class SpellData : ScriptableObject
{
    public C_Dictionary<Language, string> SpellName { get { return spellName; } }
    [SerializeField]
    private C_Dictionary<Language, string> spellName;

    [SerializeField]
    List<Spell_Tag> spellTags; //주문 태그

    public int Cost { get { return cost; } }
    [SerializeField]
    private int cost; //주문 비용

    public C_Dictionary<Value_Type, float> Values { get { return values; } }
    [SerializeField]
    private C_Dictionary<Value_Type, float> values;

    public C_Dictionary<Language, string> Description { get { return description; } }
    [SerializeField]
    private C_Dictionary<Language, string> description; // 주문 설명
}
