using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells.Enums
{
    #region basic
    public enum Spell_Type
    {
        Single_Aria,
        Multiple_Aria,
        Arcana,
        Rogue_Spell,
        Rune_Spell,
        Atri
    }

    public enum RuneSpell_Type
    {
        Basic_Rune,
        Modifier_Rune,
        Express_Rune,
    }

    public enum ValueType
    {
        Damage,
        Heal,
        duration,
        Range,
        Speed,
        Size,
        Count
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
    #endregion

    #region Arcana
    #endregion

    #region Rogue_Spell
    public enum Rogue_Elemental
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
    }
    #endregion
}
