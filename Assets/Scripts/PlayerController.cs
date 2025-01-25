using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float StompSpeed;

    public LineRenderer line;

    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 moveInput = Vector2.zero;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput = moveInput.normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0.1f;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Time.timeScale = 1;
            rigidbody.velocity = moveInput * StompSpeed;
        }
        //
    }
}
