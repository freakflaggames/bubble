using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShrimpEnemy : Enemy
{
    public SpriteRenderer spriteRenderer;
    public Sprite neutral, swim;
    public float SwimForce;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SwimForce *= 1 + LevelManager.Instance.DifficultyRange.x;
    }

    public override void Activate()
    {
        spriteRenderer.sprite = swim;
        spriteRenderer.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            spriteRenderer.transform.DOScale(1, 0.1f).SetEase(Ease.OutExpo).OnComplete(()=> {
                spriteRenderer.sprite = neutral;
            });
        });
        Vector2 player = FindAnyObjectByType<PlayerController>().transform.position;
        Vector2 dir = (player - (Vector2)transform.position).normalized;
        rb.AddForce(dir * SwimForce);
    }
}
