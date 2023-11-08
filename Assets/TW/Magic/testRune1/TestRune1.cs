using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Damages;
    using Enums;
    using TypingWizard.Spells.Utility;

    public class TestRune1 : Basic_Rune
    {
        float projectileCount = 10;
        [SerializeField]
        List<Projectile> projectiles;
        public GameObject projectilePrefab;

        public Damage damage;
        public DamageScrObj d_ScrObj;

        public override void Initialize(Character caster)
        {
            base.Initialize(caster);
            projectiles = new List<Projectile>();
            for (int i = 0; i < projectileCount; i++)
            {
                GameObject obj = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                projectiles.Add(obj.GetComponent<Projectile>());
            }

            damage = new Damage(d_ScrObj);

            modifiyableObjects.Add(new ModifiyableObject(Rune_ModifiyType.Projectile, projectiles));
        }

        public override void Cast(Character caster)
        {
            base.Cast(caster);

            Vector3 dir;
            // 모든 투사체를 360도 각도로 발사
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].isUsing)
                {
                    projectiles[i].gameObject.SetActive(true);
                    float vel = values[ValueType.Damage];
                    dir = Quaternion.Euler(0, 360f / projectileCount * i, 0) * Vector3.forward;
                    projectiles[i].Shoot(this, damage, 5f, 10f, dir);
                }
            }
        }
    }
}
