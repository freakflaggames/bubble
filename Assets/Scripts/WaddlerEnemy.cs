using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaddlerEnemy : Enemy
{
    public float rotation;
    public float moveIncrement;
    private void Start()
    {
        Vector3 diff = (transform.parent.position - transform.position).normalized;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg-90);
    }
    public override void Activate()
    {
        rotation -= moveIncrement;

        foreach(Transform child in transform.parent)
        {
            if (child.GetComponent<WaddlerEnemy>())
            {
                child.GetComponent<WaddlerEnemy>().rotation = rotation;
            }
        }

        transform.parent.DOLocalRotate(new Vector3(0, 0, rotation), 0.25f).SetEase(Ease.OutBounce);
    }
}
