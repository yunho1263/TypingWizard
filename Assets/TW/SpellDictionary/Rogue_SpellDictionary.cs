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
            // �ֹ��� �����ϴ� ���ڵ��� �����Ѵ�
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

                // �ֹ� �ڿ� ���� ���ڸ� �˻��Ѵ�
                if(!founded_Spell.CanModify(modifiers, this))
                {
                    return false;
                }

                // ��ȿ�� ���ڶ�� �ֹ��� �����ϰ� ���ڸ� �����Ѵ�
                spell = Instantiate(founded_Spell.gameObject).GetComponent<Spell>();
                spell.Initialize(Player.instance);
                Rogue_Spell r_spell = spell as Rogue_Spell;
                r_spell.Modify(modifiers, this);

                // �ֹ��� ��ȯ�Ѵ�
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
