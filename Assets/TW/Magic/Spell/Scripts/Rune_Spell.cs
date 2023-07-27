using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Enums;
    public class Rune_Spell : Spell
    {
        public RuneSpell_Type runeSpell_Type;
        public string aria;

        public List<Rune_Spell> modifiers;

        public virtual bool Modify(Rune_Spell modifier)
        {
            return true;
        }

        public void ResetSpell()
        {
            foreach (Rune_Spell modifier in modifiers)
            {
                modifier.ResetSpell();
            }
            Destroy(gameObject);
        }
    }
}
