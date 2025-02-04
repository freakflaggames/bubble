using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShrimpEnemy : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite neutral, swim;
    public float SwimForce;
    public float WaitTime;
    public int BubblesToSpawn;
    public float BubbleSpawnTime;
    public GameObject ShrimpBubblePrefab;
    int bubblesLeft = 0;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitToSwim());
    }
    public IEnumerator WaitToSwim()
    {
        yield return new WaitForSeconds(WaitTime);
        Swim();
    }
    public IEnumerator SpawnBubbles()
    {
        Transform bubble = Instantiate(ShrimpBubblePrefab, transform.position, Quaternion.identity).transform;
        bubble.SetParent(transform.parent);
        yield return new WaitForSeconds(BubbleSpawnTime);
        if (bubblesLeft > 0)
        {
            bubblesLeft--;
            StartCoroutine(SpawnBubbles());
        }
    }

    public void Swim()
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
        StartCoroutine(WaitToSwim());
        bubblesLeft = BubblesToSpawn;
        StartCoroutine(SpawnBubbles());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
