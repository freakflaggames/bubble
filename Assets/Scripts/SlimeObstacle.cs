using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlimeObstacle : MonoBehaviour
{
    private void Start()
    {
        Vector3 diff = (transform.parent.parent.position - transform.parent.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.parent.rotation = Quaternion.Euler(0, 0, deg - 90);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            transform.parent.DOScale(7.5f, 0.1f).SetEase(Ease.OutBack).OnComplete(() => { transform.parent.DOScale(6.5f, 0.1f).SetEase(Ease.OutBack); });
            AudioManager.Instance.PlaySound("slimeenter", 0.9f, 1.1f);
            collision.gameObject.GetComponent<PlayerController>().slimeJumps = 1;
            collision.gameObject.transform.position = transform.position + transform.up;
            collision.gameObject.transform.SetParent(transform);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.parent.DOScale(7.5f, 0.1f).SetEase(Ease.OutBack).OnComplete(() => { transform.parent.DOScale(6.5f, 0.1f).SetEase(Ease.OutBack); });
            AudioManager.Instance.PlaySound("slimeenter", 0.9f, 1.1f);
            collision.gameObject.GetComponent<PlayerController>().slimeJumps = 0;
            collision.gameObject.GetComponent<PlayerController>().UnAnchorPlayer();
            collision.gameObject.transform.localScale = Vector3.one * 1.5f;
        }
    }
}
