using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CannonManager : MonoBehaviour
{
    bool active;
    bool bubbleSpawned;
    public List<GameObject> Enemies;
    public Image Visual;
    public GameObject BubblePrefab;
    public GameObject BubbleBG;
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
        Visual.fillAmount = 1-((float)aliveEnemies / Enemies.Count);
        if (aliveEnemies == 0)
        {
            active = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && active && !bubbleSpawned)
        {
            Visual.transform.DORotate(new Vector3(0,0,360), 1).SetEase(Ease.OutBack);
            BubbleBG.tag = "OldBubble";
            Vector3 pos = transform.position + (Vector3)(Random.insideUnitCircle.normalized * BubbleSpawnRadius);
            print(Vector3.Distance(transform.position, pos));
            Transform bubble = Instantiate(BubblePrefab, pos, Quaternion.identity).transform;
            bubbleSpawned = true;
            collision.gameObject.GetComponent<PlayerController>().TravelToBubble(transform, bubble);
        }
    }
}
