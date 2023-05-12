using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInput playerInput;

    public float moveSpeed = 5f;

    public void OnMove(InputValue value)
    {
        transform.Translate(moveSpeed * Time.deltaTime * value.Get<Vector2>().x, 0f, moveSpeed * Time.deltaTime * value.Get<Vector2>().y);
    }
}
