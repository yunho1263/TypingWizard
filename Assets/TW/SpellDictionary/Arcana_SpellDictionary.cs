using System.Collections;
using System.Collections.Generic;
using TypingWizard.Spells;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    public class Arcana_SpellDictionary : SpellDictionary
    {
        private string firstArcana;
        private string secondArcana;

        public override void Awake()
        {
            base.Awake();
        }

        public override bool Search(List<string> words, out Spell spell)
        {
            if (words == null || words.Count > 2)
            {
                spell = null;
                return false;
            }

            if (words.Count == 1)
            {
                if (firstArcana == null)
                {
                    firstArcana = words[0];
                }
                else
                {
                    secondArcana = words[0];
                }
            }
            else
            {
                if (firstArcana == null)
                {
                    firstArcana = words[0];
                    secondArcana = words[1];
                }
                else
                {
                    firstArcana = null;
                    secondArcana = null;
                    spell = null;
                    return false;
                }
            }

            if (firstArcana != null && secondArcana != null)
            {
                Arcana_Spell founded_Spell = spellTable[firstArcana + secondArcana] as Arcana_Spell;

                if (founded_Spell != null)
                {
                    firstArcana = null;
                    secondArcana = null;
                    spell = founded_Spell;
                    return true;
                }
                else
                {
                    firstArcana = null;
                    secondArcana = null;
                }
            }

            spell = null;
            return false;
        }

        public override void AddSpell(Spell spell)
        {
            Arcana_Spell newSpell = spell as Arcana_Spell;
            spellTable.Add(newSpell.firstArcana + newSpell.secondArcana, newSpell);
        }
    }
}
