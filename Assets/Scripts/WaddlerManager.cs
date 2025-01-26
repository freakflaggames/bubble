using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaddlerManager : MonoBehaviour
{
    bool walk;
    public Sprite walk1, walk2;
    public float MoveTime;
    public float rot;
    public float increment;

    private void Start()
    {
        StartCoroutine(WaitToMove());
    }

    public IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(MoveTime);
        rot += increment;
        transform.DORotate(new Vector3(0, 0, rot), 0.25f).SetEase(Ease.OutBounce).OnComplete(()=> { 
        foreach(Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().sprite = walk ? walk1 : walk2;
                walk = !walk;
            }
        });
        StartCoroutine(WaitToMove());
    }
}
