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

    public enum Araia_Type //��â Ÿ��
    {
        SpellName,
        OneSentence,
        MultipleSentence
    }

    public int spellID; //�ֹ� ID
    public string spellName; //�ֹ� �̸�

    public bool isAcquired; //ȹ�� ����
    public List<string> arias; //�ֹ� ��â

    [SerializeField]
    public List<Spell_Tag> spellTags; //�ֹ� �±�
    public Araia_Type araiaType;

    public int cost; //�ֹ� ���

    public string description; // �ֹ� ����

    public GameObject caster; // ������
    public List<GameObject> targets; // ���

    public void Cast(GameObject caster)
    {
        Debug.Log(spellName + " "+ caster.gameObject.name);
    }
}
