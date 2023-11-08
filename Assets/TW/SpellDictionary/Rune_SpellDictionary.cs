using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    using Spells;
    using TypingWizard.Spells.Enums;

    public class Rune_SpellDictionary : SpellDictionary
    {
        private Basic_Rune currentBasicRune;
        private Rune_Spell lastRune;

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

            Rune_Spell founded_Spell = spellTable[input] as Rune_Spell;

            if (founded_Spell != null)
            {
                switch (founded_Spell.runeSpell_Type)
                {
                    case RuneSpell_Type.Basic_Rune:
                        if(currentBasicRune != null)
                        {
                            currentBasicRune.ResetSpell();
                        }
                        currentBasicRune = Instantiate(founded_Spell.gameObject).GetComponent<Spell>() as Basic_Rune;
                        currentBasicRune.Initialize(Player.instance);
                        lastRune = currentBasicRune;
                        break;

                    case RuneSpell_Type.Modifier_Rune:
                        if(lastRune != null)
                        {
                            Modifier_Rune modifier_Rune = Instantiate(founded_Spell.gameObject).GetComponent<Spell>() as Modifier_Rune;
                            modifier_Rune.Initialize(Player.instance);

                            lastRune.nextRune = modifier_Rune;
                            modifier_Rune.previousRune = lastRune;

                            lastRune = modifier_Rune;
                        }
                        break;

                    case RuneSpell_Type.Express_Rune:
                        if (lastRune != null)
                        {
                            Express_Rune express_Rune = Instantiate(founded_Spell.gameObject).GetComponent<Spell>() as Express_Rune;
                            express_Rune.Initialize(Player.instance);

                            lastRune.nextRune = express_Rune;
                            express_Rune.previousRune = lastRune;

                            if(express_Rune.GenerateSpell())
                            {
                                express_Rune.Express();
                            }
                        }
                        break;
                }

                return true;
            }
            else
            {
                lastRune = null;
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
