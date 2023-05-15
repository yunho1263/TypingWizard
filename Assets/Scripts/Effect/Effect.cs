using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public enum EffectCategori
    {
        None,
        Positive,
        Negative
    }
    public EffectCategori effectCategori;

    public enum EffectType
    {
        Burning,
        Stun,
        Slow,
        Freeze,
        Poison,
        Bleeding,
        Knockback
    }
    public EffectType effectType;

    public string effectName; // 효과 이름
    public string effectDescription; // 효과 설명

    public GameObject affects; // 영향을 받는 대상
    public GameObject caster; // 시전자
    public float duration; // 지속시간
    public float effectValue; // 효과 값

    public void ApplyEffect(GameObject target)
    {
        Debug.Log("Apply" + effectName + " " + target.gameObject.name);
    }

    public void RemoveEffect(GameObject target)
    {
        Debug.Log("Remove" + effectName + " " + target.gameObject.name);
    }

    public void UpdateEffect(GameObject target)
    {
        Debug.Log("Update" + effectName + " " + target.gameObject.name);
    }
}