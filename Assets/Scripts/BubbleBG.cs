using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBG : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
