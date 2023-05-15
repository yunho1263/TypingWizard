using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spell : MonoBehaviour
{
    public enum Spell_Tag
    {
        AttackSpell, //������
        BuffSpell, //������
        UtilitySpell, //��ƿ��Ƽ��

        Projectile, //�߻�ü
        Range, //������
        Movement, //�̵�
        Charging, //������
    }

    public enum Value_Type
    {
        Damage, //������
        Heal, //ȸ����
        Effect, //ȿ��
        Duration, //���ӽð�
        Range, //����
        Speed, //�ӵ�
    }

    public bool isAcquired;

    public string spellName;

    [SerializeField]
    public List<Spell_Tag> spellTags; //�ֹ� �±�

    public int cost; //�ֹ� ���

    public C_Dictionary<Value_Type, float> values; //�ֹ� ��

    public string description; // �ֹ� ����
    public void Cast(GameObject caster)
    {
        Debug.Log(spellName + " "+ caster.gameObject.name);
    }
}
