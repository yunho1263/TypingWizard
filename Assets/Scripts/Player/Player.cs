using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInput playerInput;

    public float moveSpeed;
    public Vector2 moveNormal;

    public TMP_InputField spellInputField;

    public void OnMove(InputValue value)
    {
        if (value.Get() == null)
        {
            moveNormal = Vector2.zero;
            return;
        }
        moveNormal = value.Get<Vector2>();
    }

    public void OnInputModeChanges() // �ֹ� �Է� �ʵ� Ȱ��ȭ
    {
        spellInputField.gameObject.SetActive(true);
        playerInput.SwitchCurrentActionMap("MagicSpell");
        spellInputField.Select();
    }

    public void OnSpellConfirm()
    {
        spellInputField.gameObject.SetActive(false);
        Debug.Log(spellInputField.text);
        playerInput.SwitchCurrentActionMap("Player");
    }

    private void Update()
    {
        // �Է¹��� �븻 ������ Translate
        transform.Translate(moveNormal * moveSpeed * Time.deltaTime);
    }
}
