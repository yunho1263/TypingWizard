using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    using Spells;

    public class Single_SpellDictionary : SpellDictionary
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        public override bool Search(string input, out Spell spell)
        {
            if (spellTable == null)
            {
                spell = null;
                return false;
            }

            SingleAria_Spell Founded_Spell = spellTable[input] as SingleAria_Spell;

            if (Founded_Spell != null)
            {
                spell = Founded_Spell;
                return true;
            }

            spell = null;
            return false;
        }

        public override void AddSpell(Spell spell)
        {
            SingleAria_Spell newSpell = spell as SingleAria_Spell;
            spellTable.Add(newSpell.aria, newSpell);
        }
    }
}
