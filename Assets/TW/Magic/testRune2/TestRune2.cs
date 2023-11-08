using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Damages;
    using Enums;
    using TypingWizard.Spells.Utility;
    using UnityEngine.UIElements;

    public class TestRune2 : Modifier_Rune
    {
        public Damage damage;
        public DamageScrObj d_ScrObj;

        public override void Initialize(Character caster)
        {
            base.Initialize(caster);

            damage = new Damage(d_ScrObj);
        }

        public override bool Modify()
        {
            if (previousRune.spellTags.Contains(Spell_Tag.Projectile))
            {
                List<Rune_ModifiyType> modifiyTypes = new List<Rune_ModifiyType>
                {
                    Rune_ModifiyType.Projectile
                };

                List<ModifiyableObject> modifiyableObjects = previousRune.ModifiyRequest(modifiyTypes);

                if (modifiyableObjects != null)
                {
                    Modify(modifiyableObjects[0].target as List<Projectile>);

                    // 다음 룬에 모디파이 신호 전달
                    return base.Modify();
                }
            }
            return false;
        }

        public override void Modify(List<Projectile> projectiles)
        {
            base.Modify(projectiles);

            foreach (Projectile projectile in projectiles)
            {
                projectile.OnHit.AddListener(OnHit);
            }
        }

        public void OnHit()
        {
            Debug.Log("TestRune2 Hit");
        }
    }
}
