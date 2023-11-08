using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TypingWizard.Spells
{
    using Enums;
    using Utility;

    public class Modifier_Rune : Rune_Spell
    {
        public override RuneSpell_Type runeSpell_Type => RuneSpell_Type.Modifier_Rune;
        public List<Rune_ModifiyType> modifiyTypes;

        public virtual bool Modify()
        {
            previousRune.modifiys.Add(this);
            if (previousRune.runeSpell_Type == RuneSpell_Type.Modifier_Rune)
            {
                Modifier_Rune modifier_Rune = previousRune as Modifier_Rune;

                if (modifier_Rune.Modify())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public virtual void Modify(Rune_Spell targetSpell)
        {

        }

        public virtual void Modify(List<Projectile> projectiles)
        {

        }

        public virtual void Modify(UnityEvent unityEvent)
        {

        }
    }
}
