using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    using Spells;

    public class Single_SpellDictionary : SpellDictionary
    {
        public Single_SpellDictionary()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override bool Search(string input, out Spell spell)
        {
            spell = null;
            if (spellTable == null)
            {
                return false;
            }

            SingleAria_Spell founded_Spell = spellTable[input] as SingleAria_Spell;

            if (founded_Spell != null)
            {
                spell = GameObject.Instantiate(founded_Spell).GetComponent<Spell>();
                spell.Initialize(Player.instance);
                return true;
            }

            return false;
        }

        public override void AddSpell(Spell spell)
        {
            SingleAria_Spell newSpell = spell as SingleAria_Spell;
            spellTable.Add(newSpell.aria, newSpell);
        }
    }
}
