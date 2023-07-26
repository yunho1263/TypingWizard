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
        AttackSpell, //공격형
        BuffSpell, //버프형
        UtilitySpell, //유틸리티형

        Projectile, //발사체
        Range, //범위형
        Movement, //이동
    }
}
