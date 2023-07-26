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

        public override void Initialize()
        {
            base.Initialize();
        }

        public override bool Search(List<string> words, out Spell spell)
        {
            if (words == null || words.Count > 2)
            {
                spell = null;
                return false;
            }

            // 여기 수정할것
            firstArcana = words[0];
            secondArcana = words[1];

            Arcana_Spell Founded_Spell = spellTable[firstArcana + secondArcana] as Arcana_Spell;

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
            Arcana_Spell newSpell = spell as Arcana_Spell;
            spellTable.Add(newSpell.firstArcana + newSpell.secondArcana, newSpell);
        }
    }
}
