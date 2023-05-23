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

    public string effectName; // ȿ�� �̸�
    public string effectDescription; // ȿ�� ����

    public GameObject caster; // ������
    public GameObject target; // ������ �޴� ���
    public float duration; // ���ӽð�
    public float effectValue; // ȿ�� ��


    public UnityEvent<GameObject> OnApplyEffect; // ȿ�� �����
    public void ApplyEffect(GameObject target)
    {
        OnApplyEffect.Invoke(target);
    }

    public UnityEvent<GameObject> OnRemoveEffect; // ȿ�� ���Ž�
    public void RemoveEffect(GameObject target)
    {
        OnRemoveEffect.Invoke(target);
    }

    public UnityEvent<GameObject> OnUpdateEffect; // ȿ�� ������Ʈ��
    public void UpdateEffect(GameObject target)
    {
        OnUpdateEffect.Invoke(target);
    }
}