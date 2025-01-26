using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperEnemyManager : MonoBehaviour
{
    public float WaitTime;
    public JumperEnemy[] enemies;
    public int index;

    private void Start()
    {
    }
    IEnumerator BeginJumpCycle()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(WaitToJump());
    }
    IEnumerator WaitToJump()
    {
        if (enemies[index] != null)
        {
            enemies[index].Jump();
        }
        index++;
        index %= enemies.Length;
        yield return new WaitForSeconds(WaitTime);
        StartCoroutine(WaitToJump());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BeginJumpCycle());
        }
    }
}
