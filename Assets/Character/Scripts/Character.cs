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

    public string characterName; // 캐릭터 이름
    public string characterDescription; // 캐릭터 설명
    public string characterSubtitle; // 캐릭터 부제

    public float maxHp; // 최대 체력    
    public float currentHp; // 현재 체력
    public float maxMp; // 최대 마나
    public float currentMp; // 현재 마나

    public float moveSpeed; // 이동 속도
    public Vector2 moveDirNomormal; // 이동 방향

    protected void Start()
    {
        Initialize(); // 초기화
    }

    public void Initialize()
    {
        currentHp = maxHp; // 현재 체력을 최대 체력으로 초기화
        currentMp = maxMp; // 현재 마나를 최대 마나로 초기화

        moveDirNomormal = Vector2.zero; // 이동 방향을 0으로 초기화
    }


    public UnityEvent<Damage, float> onDamage; // 데미지를 받았을 때 이벤트
    public void TakeDamage(Damage damage) // 데미지를 받는 함수
    {
        float damageValue = damage.damageValue; // 데미지 값을 저장

        currentHp -= damageValue; // 현재 체력에서 데미지만큼 감소

        damage.OnApplyDamage.Invoke(this); // 데미지 객체가 데미지를 주었을 떄 이벤트 발생
        onDamage.Invoke(damage, damageValue); // 데미지를 받았을 때 이벤트 발생

        if (currentHp <= 0) // 체력이 0 이하일 때
        {
            Die(damage.owner); // 죽는 함수 실행
        }
    }

    public UnityEvent<Heal, float> onHealing; // 회복을 받았을 때 이벤트
    public void Heal(Heal heal) // 회복을 받는 함수
    {
        float healValue = heal.value; // 회복량을 저장
        // 회복량이 최대 체력을 넘어설 때 초과값을 오버 힐링으로 처리
        if (currentHp + healValue > maxHp)
        {
            float overHealValue = currentHp + healValue - maxHp; // 초과값을 오버 힐링으로 처리
            healValue = maxHp - currentHp; // 회복량을 최대 체력에서 현재 체력을 뺀 값으로 설정

            currentHp = maxHp; // 현재 체력을 최대 체력으로 설정
            OverHealing(heal, overHealValue); // 오버 힐링 함수 실행
        }

        currentHp += healValue; // 현재 체력에서 회복만큼 증가

        heal.OnApplyHeal.Invoke(this); // 회복 객체가 회복을 주었을 때 이벤트 발생
        onHealing.Invoke(heal, healValue); // 회복을 받았을 때 이벤트 발생
    }

    public UnityEvent<Heal, float> onOverHealing; // 최대체력을 넘어선 회복이 일어났을 때 이벤트
    public void OverHealing(Heal heal, float overVal) // 오버 힐링을 받는 함수
    {
        onOverHealing.Invoke(heal, overVal); // 오버 힐링 이벤트 발생
    }

    public UnityEvent<Character> onDeath; // 죽었을 때 이벤트
    public void Die(Character killer) // 죽는 함수
    {
        onDeath.Invoke(killer); // 죽음 이벤트 발생
    }

    public UnityEvent onRevive; // 부활했을 때 이벤트
    public void Revive() // 부활하는 함수
    {
        onRevive.Invoke(); // 부활 이벤트 발생
    }

    public void Move() // 이동하는 함수
    {
        //월드 좌표계를 기준으로 translete 함수를 사용하여 이동
        transform.Translate(moveDirNomormal * moveSpeed * Time.deltaTime, Space.World);
    }
}
