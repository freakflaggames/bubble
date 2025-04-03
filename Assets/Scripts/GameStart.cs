using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameStart : MonoBehaviour
{
    public Transform startOverlay;
    public float moveAmount;
    public float moveTime;
    private void Start()
    {
        startOverlay.transform.DOLocalMoveX(moveAmount, moveTime).SetEase(Ease.OutBack);
    }
}
