using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public enum CharacterType
    {
        Player,
        Enemy
    }

    public CharacterType characterType;

    public string characterName; // ĳ���� �̸�
    public string characterDescription; // ĳ���� ����
    public string characterSubtitle; // ĳ���� ����

    public float maxHp; // �ִ� ü��    
    public float currentHp; // ���� ü��
    public float maxMp; // �ִ� ����
    public float currentMp; // ���� ����

    public float moveSpeed; // �̵� �ӵ�
    public Vector2 moveDirNomormal; // �̵� ����

    protected void Start()
    {
        Initialize(); // �ʱ�ȭ
    }

    public void Initialize()
    {
        currentHp = maxHp; // ���� ü���� �ִ� ü������ �ʱ�ȭ
        currentMp = maxMp; // ���� ������ �ִ� ������ �ʱ�ȭ

        moveDirNomormal = Vector2.zero; // �̵� ������ 0���� �ʱ�ȭ
    }


    public UnityEvent<Damage, float> onDamage; // �������� �޾��� �� �̺�Ʈ
    public void TakeDamage(Damage damage) // �������� �޴� �Լ�
    {
        float damageValue = damage.damageValue; // ������ ���� ����

        currentHp -= damageValue; // ���� ü�¿��� ��������ŭ ����

        damage.OnApplyDamage.Invoke(this); // ������ ��ü�� �������� �־��� �� �̺�Ʈ �߻�
        onDamage.Invoke(damage, damageValue); // �������� �޾��� �� �̺�Ʈ �߻�

        if (currentHp <= 0) // ü���� 0 ������ ��
        {
            Die(damage.owner); // �״� �Լ� ����
        }
    }

    public UnityEvent<Heal, float> onHealing; // ȸ���� �޾��� �� �̺�Ʈ
    public void Heal(Heal heal) // ȸ���� �޴� �Լ�
    {
        float healValue = heal.value; // ȸ������ ����
        // ȸ������ �ִ� ü���� �Ѿ �� �ʰ����� ���� �������� ó��
        if (currentHp + healValue > maxHp)
        {
            float overHealValue = currentHp + healValue - maxHp; // �ʰ����� ���� �������� ó��
            healValue = maxHp - currentHp; // ȸ������ �ִ� ü�¿��� ���� ü���� �� ������ ����

            currentHp = maxHp; // ���� ü���� �ִ� ü������ ����
            OverHealing(heal, overHealValue); // ���� ���� �Լ� ����
        }

        currentHp += healValue; // ���� ü�¿��� ȸ����ŭ ����

        heal.OnApplyHeal.Invoke(this); // ȸ�� ��ü�� ȸ���� �־��� �� �̺�Ʈ �߻�
        onHealing.Invoke(heal, healValue); // ȸ���� �޾��� �� �̺�Ʈ �߻�
    }

    public UnityEvent<Heal, float> onOverHealing; // �ִ�ü���� �Ѿ ȸ���� �Ͼ�� �� �̺�Ʈ
    public void OverHealing(Heal heal, float overVal) // ���� ������ �޴� �Լ�
    {
        onOverHealing.Invoke(heal, overVal); // ���� ���� �̺�Ʈ �߻�
    }

    public UnityEvent<Character> onDeath; // �׾��� �� �̺�Ʈ
    public void Die(Character killer) // �״� �Լ�
    {
        onDeath.Invoke(killer); // ���� �̺�Ʈ �߻�
    }

    public UnityEvent onRevive; // ��Ȱ���� �� �̺�Ʈ
    public void Revive() // ��Ȱ�ϴ� �Լ�
    {
        onRevive.Invoke(); // ��Ȱ �̺�Ʈ �߻�
    }

    public void Move() // �̵��ϴ� �Լ�
    {
        //���� ��ǥ�踦 �������� translete �Լ��� ����Ͽ� �̵�
        transform.Translate(moveDirNomormal * moveSpeed * Time.deltaTime, Space.World);
    }
}
