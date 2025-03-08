using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperObstacle : MonoBehaviour
{
    public float LaunchForce;
    private void Start()
    {
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg - 90);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().UnAnchorPlayer();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * LaunchForce;
        }
    }
}
