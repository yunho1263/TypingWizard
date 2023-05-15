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

    public string effectName; // ȿ�� �̸�
    public string effectDescription; // ȿ�� ����

    public GameObject affects; // ������ �޴� ���
    public GameObject caster; // ������
    public float duration; // ���ӽð�
    public float effectValue; // ȿ�� ��

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