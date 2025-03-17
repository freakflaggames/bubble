using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BumperObstacle : MonoBehaviour
{
    public float angle;
    public float LaunchForce;
    bool backwards;
    private void Start()
    {
        backwards = Random.Range(0, 2) == 0;
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg - 90 + (angle * (backwards ? -1 : 1)));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.DOScale(new Vector2(3,1.5f), 0.05f).SetEase(Ease.OutBack).OnComplete(() => { transform.DOScale(2, 0.05f).SetEase(Ease.OutBounce); });
            AudioManager.Instance.PlaySound("pinball", 1, 1f);
            collision.gameObject.GetComponent<PlayerController>().UnAnchorPlayer();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * LaunchForce;
        }
    }
}
