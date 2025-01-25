using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperEnemy : MonoBehaviour
{
    public float JumpForce;
    public float JumpTime;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg);
        StartCoroutine(WaitToJump());
    }
    IEnumerator WaitToJump()
    {
        yield return new WaitForSeconds(JumpTime);
        Jump();
    }
    void Jump()
    {
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        rb.velocity = JumpForce * diff;
        StartCoroutine(WaitToJump());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 diff = (transform.parent.position - transform.position).normalized;
            float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, deg);
            rb.velocity = Vector3.zero;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
