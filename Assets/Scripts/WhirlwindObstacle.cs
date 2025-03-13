using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlwindObstacle : MonoBehaviour
{
    Transform whirlwindParent;
    public float rotateSpeed;
    bool clockwise;
    float rot;
    void Start()
    {
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg - 90);

        whirlwindParent = Instantiate(new GameObject(), transform.parent).transform;
        whirlwindParent.transform.position = transform.parent.position;
        transform.SetParent(whirlwindParent.transform);

        clockwise = Random.Range(0, 2) == 0;
    }
    private void Update()
    {
        rot += Time.deltaTime * rotateSpeed * (clockwise ? 1 : -1);
        whirlwindParent.transform.rotation = Quaternion.Euler(0, 0, rot);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = transform.position;
            collision.gameObject.transform.SetParent(transform);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
