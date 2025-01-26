using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShrimpBubble : MonoBehaviour
{
    public float VelocityDecayRate;
    float scale = 2;
    private void Update()
    {
        scale -= Time.deltaTime;
        transform.localScale = Vector3.one * scale;
        if (scale <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity /= VelocityDecayRate;
        }
    }
}
