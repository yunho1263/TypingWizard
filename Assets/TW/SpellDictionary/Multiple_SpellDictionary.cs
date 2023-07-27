using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    using Spells;

    public class Multiple_SpellDictionary : SpellDictionary
    {
        private MultipleAria_Spell currentcastingSpell;
        private int currentAriaIndex;

        public Multiple_SpellDictionary()
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

            if (currentcastingSpell != null)
            {
                // 캐스팅 중인 주문의 순서에 맞는 영창이면
                if (currentcastingSpell.arias[currentAriaIndex].Equals(input))
                {
                    // 마지막 영창이라면 주문 시전 성공
                    if (currentcastingSpell.arias.Count == ++currentAriaIndex)
                    {
                        spell = GameObject.Instantiate(currentcastingSpell).GetComponent<Spell>();
                        spell.Initialize(Player.instance);
                        currentcastingSpell = null;
                        currentAriaIndex = 0;
                        return true;
                    }
                }
                else // 현재 순서인 영창과 틀리면 실패
                {
                    currentcastingSpell = null;
                    currentAriaIndex = 0;
                }
            }

            MultipleAria_Spell Founded_Spell = spellTable[input] as MultipleAria_Spell;

            if (Founded_Spell != null)
            {
                currentcastingSpell = Founded_Spell;
                currentAriaIndex = 1;
            }

            return false;
        }

        public override void AddSpell(Spell spell)
        {
            MultipleAria_Spell newSpell = spell as MultipleAria_Spell;
            spellTable.Add(newSpell.arias[0], newSpell);
        }
    }
}
