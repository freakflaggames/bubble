using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    bool active;
    bool bubbleSpawned;
    public List<GameObject> Enemies;
    public GameObject Visual;
    public GameObject BubblePrefab;
    public float BubbleSpawnRadius;

    private void Update()
    {
        int aliveEnemies = 0;
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i] != null)
            {
                aliveEnemies++;
            }
        }
        if (aliveEnemies == 0)
        {
            Visual.SetActive(true);
            active = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && active && !bubbleSpawned)
        {
            Vector3 pos = transform.position + (Vector3)(Random.insideUnitCircle.normalized * BubbleSpawnRadius);
            print(Vector3.Distance(transform.position, pos));
            Transform bubble = Instantiate(BubblePrefab, pos, Quaternion.identity).transform;
            bubbleSpawned = true;
            collision.gameObject.GetComponent<PlayerController>().TravelToBubble(bubble);
        }
    }
}
