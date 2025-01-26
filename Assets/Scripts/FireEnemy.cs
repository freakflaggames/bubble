using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireEnemy : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite neutral, shoot;
    public float ShootForce;
    public GameObject Fireball;
    public Transform ShootPoint;
    public float WaitTime;
    public float PauseTime;
    bool active;
    private void Start()
    {
        StartCoroutine(WaitToPause());
        StartCoroutine(WaitToShoot());
    }
    IEnumerator WaitToPause()
    {
        active = !active;
        yield return new WaitForSeconds(PauseTime);
        StartCoroutine(WaitToPause());
    }
    IEnumerator WaitToShoot()
    {
        yield return new WaitForSeconds(WaitTime);
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
            fireball.GetComponent<Rigidbody2D>().velocity = transform.up * ShootForce;
        }
        StartCoroutine(WaitToShoot());
    }
}
