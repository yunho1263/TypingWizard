using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public GameObject caster; // 시전자
    public GameObject target; // 영향을 받는 대상
    public float duration; // 지속시간
    public float effectValue; // 효과 값


    public UnityEvent<GameObject> OnApplyEffect; // 효과 적용시
    public void ApplyEffect(GameObject target)
    {
        OnApplyEffect.Invoke(target);
    }

    public UnityEvent<GameObject> OnRemoveEffect; // 효과 제거시
    public void RemoveEffect(GameObject target)
    {
        OnRemoveEffect.Invoke(target);
    }

    public UnityEvent<GameObject> OnUpdateEffect; // 효과 업데이트시
    public void UpdateEffect(GameObject target)
    {
        OnUpdateEffect.Invoke(target);
    }
}