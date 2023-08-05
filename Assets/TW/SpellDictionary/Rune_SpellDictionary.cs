using System.Collections;
using System.Collections.Generic;
using TypingWizard.Spells;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    public class Rune_SpellDictionary : SpellDictionary
    {
        private Rune_Spell currentBasicRune;
        private Rune_Spell lastRune;

        public override void Awake()
        {
            base.Awake();
        }

        public override bool Search(string input, out Spell spell)
        {
            spell = null;
            if (spellTable == null)
            {
                return false;
            }

            Rune_Spell founded_Spell = spellTable[input] as Rune_Spell;

            if (founded_Spell != null)
            {
                switch (founded_Spell.runeSpell_Type)
                {
                    case Spells.Enums.RuneSpell_Type.Basic_Rune:
                        break;

                    case Spells.Enums.RuneSpell_Type.Modifier_Rune:
                        break;

                    case Spells.Enums.RuneSpell_Type.Express_Rune:
                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public override void AddSpell(Spell spell)
        {
            Rune_Spell newSpell = spell as Rune_Spell;
            spellTable.Add(newSpell.aria, newSpell);
        }
    }
}
