using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCycleManager : MonoBehaviour
{
    public float WaitTime;
    public List<GameObject> Enemies = new List<GameObject>();
    public int index;

    private void Start()
    {
        WaitTime /= 1+LevelManager.Instance.DifficultyRange.x;
    }
    IEnumerator BeginCycle()
    {
        float beginWaitTime = 0.5f / (1 + LevelManager.Instance.DifficultyRange.x);
        yield return new WaitForSeconds(beginWaitTime);
        StartCoroutine(ActivateInSequence());
    }
    IEnumerator ActivateInSequence()
    {
        if (Enemies[index] != null)
        {
            Enemies[index].GetComponent<Enemy>().Activate();
        }
        index++;
        index %= Enemies.Count;
        yield return new WaitForSeconds(WaitTime);
        StartCoroutine(ActivateInSequence());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when player enters bubble, begin cycle
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BeginCycle());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }
}
