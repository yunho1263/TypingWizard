using System.Collections;
using System.Collections.Generic;
using TypingWizard.Spells;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    public class Rogue_SpellDictionary : SpellDictionary
    {
        public Rogue_SpellDictionary()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override bool Search(List<string> words, out Spell spell)
        {
            spell = null;
            if (spellTable == null || words.Count < 2)
            {
                return false;
            }

            Rogue_Spell founded_Spell = spellTable[words[0]] as Rogue_Spell;

            if (founded_Spell != null)
            {
                List<string> modifiers = words.GetRange(1, words.Count - 1);

                foreach (string modifier in modifiers)
                {
                    if (!founded_Spell.Modify(modifier))
                    {
                        return false;
                    }
                }
                
                spell = founded_Spell;
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void AddSpell(Spell spell)
        {
            Rogue_Spell rogue_Spell = spell as Rogue_Spell;
            spellTable.Add(rogue_Spell.aria, rogue_Spell);
        }
    }
}
