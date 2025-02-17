using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireEnemy : Enemy
{
    public SpriteRenderer spriteRenderer;
    public Sprite neutral, shoot;
    public float ShootForce;
    public GameObject Fireball;
    public Transform ShootPoint;
    public float WaitTime;
    public bool active;
    private void Start()
    {
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg-90);
        StartCoroutine(WaitToShoot());
    }
    public override void Activate()
    {
        active = !active;
    }
    IEnumerator WaitToShoot()
    {
        yield return new WaitForSeconds(WaitTime / (1 + LevelManager.Instance.DifficultyRange.x));
        if (active)
        {
            spriteRenderer.sprite = shoot;
            spriteRenderer.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.1f).SetEase(Ease.OutExpo).OnComplete(() => {
                    spriteRenderer.sprite = neutral;
                });
            });
            GameObject fireball = Instantiate(Fireball, ShootPoint.position, Quaternion.identity);
            fireball.transform.SetParent(transform.parent.parent);
            fireball.GetComponent<Rigidbody2D>().velocity = ShootPoint.up * ShootForce * (1 + LevelManager.Instance.DifficultyRange.x);
        }
        StartCoroutine(WaitToShoot());
    }
}
