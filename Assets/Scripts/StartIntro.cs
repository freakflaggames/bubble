using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class StartIntro : MonoBehaviour
{
    public CinemachineVirtualCamera cvc;
    public Transform firstBubble;
    float lensSize;
    bool disabled;
    private void Start()
    {
        lensSize = cvc.m_Lens.OrthographicSize;
    }
    public void Intro()
    {
        cvc.Follow = firstBubble;
        DOTween.To(() => lensSize, x => lensSize = x, 9, 1).SetEase(Ease.OutExpo).OnComplete(()=> { disabled = true; });
    }
    private void Update()
    {
        if (!disabled)
        {
            cvc.m_Lens.OrthographicSize = lensSize;
        }
    }
}
