using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class WipeTransition : MonoBehaviour
{
    public Transform startOverlay;
    public float moveAmount;
    public float moveTime;
    public bool start;
    public bool loadScene;

    private void Start()
    {
        if (start)
        {
            Wipe();
        }
    }
    public void Wipe()
    {
        startOverlay.transform.DOLocalMoveX(moveAmount, moveTime).OnComplete(()=> {
            if (loadScene)
            {
                SceneManager.LoadScene("SampleScene");
            }
        });
    }
}
