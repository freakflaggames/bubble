using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumperEnemy : Enemy
{
    public int trajectoryRatio;
    public Sprite neutral, jump;
    public SpriteRenderer spriteRenderer;
    public float JumpForce;
    public Vector2 JumpForceRange;
    float startJumpForce;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg-90);
        JumpForce *= 1 + LevelManager.Instance.DifficultyRange.x;
        JumpForce = Mathf.Clamp(JumpForce, JumpForceRange.x, JumpForceRange.y);
    }
    public override void Activate()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (trajectoryRatio == 0)
        {
            trajectoryRatio = transform.parent.childCount;
        }
        if (rb.velocity == Vector2.zero)
        {
            spriteRenderer.sprite = jump;
            Vector3 diff = (transform.up + transform.right / trajectoryRatio).normalized;
            rb.velocity = JumpForce * diff;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            spriteRenderer.transform.DOScale(new Vector2(1.5f, .5f), 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.15f).SetEase(Ease.OutExpo);
            });
            spriteRenderer.sprite = neutral;
            Vector3 diff = (transform.parent.position - transform.position).normalized;
            float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, deg-90);
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
