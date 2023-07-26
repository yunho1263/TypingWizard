using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells.Enums
{
    public enum Spell_Type
    {
        Single_Aria,
        Multiple_Aria,
        Arcana,
        Rogue_Spell,
        Rune_Spell,
        Atri
    }

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
    }
}