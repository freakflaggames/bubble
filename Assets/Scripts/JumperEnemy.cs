using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumperEnemy : MonoBehaviour
{
    public Sprite neutral, jump;
    public SpriteRenderer spriteRenderer;
    public float JumpForce;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg);
    }
    public void Jump()
    {
        if (rb.velocity == Vector2.zero)
        {
            spriteRenderer.sprite = jump;
            Vector3 diff = (transform.parent.position - transform.position).normalized;
            rb.velocity = JumpForce * diff;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            spriteRenderer.transform.DOScale(new Vector2(2, .5f), 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.15f).SetEase(Ease.OutExpo);
            });
            spriteRenderer.sprite = neutral;
            Vector3 diff = (transform.parent.position - transform.position).normalized;
            float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, deg);
            rb.velocity = Vector3.zero;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy(gameObject);
        }
    }
}
