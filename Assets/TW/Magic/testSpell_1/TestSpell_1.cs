using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Enums;
    using Damages;

    public class TestSpell_1 : SingleAria_Spell
    {
        public int maxProjectileCount;

        [SerializeField]
        List<Projectile> projectiles;
        public GameObject projectilePrefab;

        public Damage damage;
        public DamageScrObj d_ScrObj;

        public override void Initialize(Character caster)
        {
            base.Initialize(caster);
            projectiles = new List<Projectile>();
            for (int i = 0; i < maxProjectileCount; i++)
            {
                GameObject obj = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                projectiles.Add(obj.GetComponent<Projectile>());
            }

            damage = new Damage(d_ScrObj);
        }

        public override void Cast(Character caster)
        {
            base.Cast(caster);
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].isUsing)
                {
                    projectiles[i].gameObject.SetActive(true);
                    float vel = values[ValueType.Damage];
                    projectiles[i].Shoot(this, damage, 5f, 10f, Vector3.forward);
                    break;
                }
            }
        }
    }

}
