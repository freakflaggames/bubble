using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CannonManager : MonoBehaviour
{
    //TODO: make a separate level/bubble class
    bool active;
    bool bubbleSpawned;
    public List<GameObject> Keys;
    //public GameObject[] BubblePrefabs; 
    public GameObject BubbleBG;
    public GameObject StarBurst;
    public SpriteRenderer spriteRenderer;
    public Sprite bgOverlay;
    public Sprite chain2, chain1, nochain, opened;
    public float BubbleSpawnRadius;
    private void Start()
    {
        //get bubble bg from bubble parent transform
        BubbleBG = transform.parent.parent.Find("BG").gameObject;

        //when generated, set bg color to this bubble's color (move to bubble class)
        FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>().bgOverlay.DOColor(new Color(1, 1, 1, 1), 1).OnComplete(() =>
        {
            FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>().bgOverlay.DOColor(new Color(1, 1, 1, 0), 1).OnComplete(() =>
            {
                FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>().bgOverlay.DOColor(new Color(1, 1, 1, 0), .4f).OnComplete(() =>
                {
                    FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>().bgOverlay.sprite = bgOverlay;
                    FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>().bgOverlay.DOColor(new Color(1, 1, 1, 1), 1);
                });
            });
        });
    }
    private void Update()
    {
        //TODO: simplify and move out of update
        int aliveEnemies = 0;
        for (int i = 0; i < Keys.Count; i++)
        {
            if (Keys[i] != null)
            {
                aliveEnemies++;
            }
        }
        float percentage = 1 - ((float)aliveEnemies / Keys.Count);
        if (percentage > 1/3 && spriteRenderer.sprite == chain2)
        {
            spriteRenderer.sprite = chain1;
            spriteRenderer.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.1f).SetEase(Ease.OutExpo).OnComplete(() => {
                    
                });
            });
        }
        else if (percentage > 2/3 && spriteRenderer.sprite == chain1)
        {
            spriteRenderer.sprite = nochain;
            spriteRenderer.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.1f).SetEase(Ease.OutExpo).OnComplete(() => {
                    
                });
            });
        }
        else if (percentage == 1 && spriteRenderer.sprite == nochain)
        {
            Instantiate(StarBurst, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySound("keyscollected", 1, 1);
            spriteRenderer.sprite = opened;
            spriteRenderer.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.1f).SetEase(Ease.OutExpo);
            });
            active = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: move to bubble class
        if (collision.gameObject.CompareTag("Player") && active && !bubbleSpawned)
        {
            AudioManager.Instance.PlaySound("load", 1, 1);
            spriteRenderer.sortingOrder = 5;
            BubbleBG.tag = "OldBubble";
            Vector3 pos = transform.position + (Vector3)(Random.insideUnitCircle.normalized * BubbleSpawnRadius);
            Vector3 diff = (transform.position - pos).normalized;
            float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.DORotate(new Vector3(0, 0, deg), 1).SetEase(Ease.OutBounce);
            //Transform bubble = Instantiate(BubblePrefabs[Random.Range(0,BubblePrefabs.Length)], pos, Quaternion.identity).transform;
            Transform bubble = Instantiate(LevelManager.Instance.GetRandomLevel(), pos, Quaternion.identity).transform;
            bubbleSpawned = true;
            collision.gameObject.GetComponent<PlayerController>().TravelToBubble(transform, bubble);
        }
    }
}
