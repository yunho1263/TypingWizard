using System;
using System.Collections;
using System.Collections.Generic;
using TypingWizard.Spells;
using TypingWizard.Spells.Utility;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace TypingWizard.SpellDictionary
{
    using Spells.Enums;
    public class Rogue_SpellDictionary : SpellDictionary
    {
        public RogueSpell_Arguments<Rogue_Elemental> elementalType;

        public override void Awake()
        {
            base.Awake();
            // 주문을 수식하는 인자들을 정의한다
            StringTableCollection stringTableCol_REET = LocalizationEditorSettings.GetStringTableCollection("RogueSpell_ElementTypes");
            elementalType = new RogueSpell_Arguments<Rogue_Elemental>(stringTableCol_REET);
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

                // 주문 뒤에 붙은 인자를 검사한다
                if(!founded_Spell.CanModify(modifiers, this))
                {
                    return false;
                }

                // 우효한 인자라면 주문을 생성하고 인자를 적용한다
                spell = Instantiate(founded_Spell.gameObject).GetComponent<Spell>();
                spell.Initialize(Player.instance);
                Rogue_Spell r_spell = spell as Rogue_Spell;
                r_spell.Modify(modifiers, this);

                // 주문을 반환한다
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
