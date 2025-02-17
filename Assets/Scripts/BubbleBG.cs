using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BubbleBG : MonoBehaviour
{
    GameObject player;
    bool startWobble = true, endWobble = false;
    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject;
    }
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 8 && startWobble)
        {
            Wobble();
            if (!gameObject.CompareTag("OldBubble"))
            {
                endWobble = true;
            }
            startWobble = false;
        }
        if (distance > 8)
        {
            if (endWobble)
            {
                Wobble();
                endWobble = false;
            }
            startWobble = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(transform.parent.gameObject);
        }
    }
    void Wobble()
    {
        float startScale = 0.47f;
        AudioManager.Instance.PlaySound("bounce", 0.9f, 1.1f);
        transform.DOScale(new Vector2(startScale*1.1f, startScale*0.9f), 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            transform.DOScale(new Vector2(startScale*0.9f, startScale*1.1f), 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                transform.DOScale(new Vector2(startScale, startScale), 0.2f).SetEase(Ease.OutExpo);
            });
        });
    }
}
