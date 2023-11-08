using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Enums;
    public class Express_Rune : Rune_Spell
    {
        public override RuneSpell_Type runeSpell_Type => RuneSpell_Type.Express_Rune;

        public virtual bool GenerateSpell()
        {
            if (previousRune.runeSpell_Type == RuneSpell_Type.Modifier_Rune)
            {
                Modifier_Rune modifier_Rune = previousRune as Modifier_Rune;

                if (modifier_Rune.Modify())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public virtual void Express()
        {

        }
    }
}
