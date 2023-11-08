using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Damages;
    using UnityEngine.Events;

    public class Projectile : MonoBehaviour
    {
        public Vector3 normalDir;
        public float speed = 10f;
        public float lifeTime = 5f;
        public Damage damage;
        public GameObject impactEffect;

        public bool isUsing;

        public Character owner;

        public UnityEvent OnHit;

        private void Start()
        {

        }

        private void Update()
        {
            transform.Translate(normalDir * speed * Time.deltaTime);
        }

        public IEnumerator LifeTime()
        {
            yield return new WaitForSeconds(lifeTime);
            isUsing = false;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Hit Enemy");
            }
            OnHit.Invoke();
            //Instantiate(impactEffect, transform.position, transform.rotation);
        }

        public void Shoot(Spell spell, Damage damage, float lifeTime, float speed, Vector3 nDir)
        {
            owner = spell.caster.GetComponent<Character>();
            transform.position = spell.caster.transform.position;
            damage.Initialize(spell, damage);
            this.lifeTime = lifeTime;
            this.speed = speed;
            normalDir = nDir;
            StartCoroutine(LifeTime());
        }

        public void SetDir(Vector3 nDir)
        {
            normalDir = nDir;
        }
    }
}
