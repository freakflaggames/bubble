using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class GameOver : MonoBehaviour
{
    public GameObject starBurst;
    public float scoreAddTime, scoreAddTimeMax;
    public float timeModifier;
    public float pitch;
    public int keysLeft, worldsLeft;
    public int keys, worlds, score;
    public TextMeshProUGUI worldScore, keyScore, scoreText;
    private void Start()
    {
        worldsLeft = PlayerPrefs.GetInt("worlds");
        keysLeft = PlayerPrefs.GetInt("keys");
        StartCoroutine(WaitToAddScore());
    }
    private void Update()
    {
        worldScore.text = worlds + "";
        keyScore.text = keys + "";
        scoreText.text = score + "";
    }
    IEnumerator WaitToAddScore()
    {
        yield return new WaitForSeconds(.5f);
        StartCoroutine(AddWorldScore());
    }
    IEnumerator AddWorldScore()
    {
        yield return new WaitForSeconds(scoreAddTime);
        AudioManager.Instance.PlaySound("bounce", pitch, pitch);
        pitch += 0.1f;
        worlds++;
        worldsLeft--;
        score += 100;
        scoreText.transform.DOScale(1.1f, scoreAddTime/2).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            scoreText.transform.DOScale(1, scoreAddTime / 2).SetEase(Ease.OutExpo);
        });
        scoreAddTime /= timeModifier;
        if (worldsLeft > 0)
        {
            StartCoroutine(AddWorldScore());
        }
        else
        {
            pitch = 1;
            scoreAddTime = scoreAddTimeMax;
            StartCoroutine(AddKeyScore());
        }
    }
    IEnumerator AddKeyScore()
    {
        AudioManager.Instance.PlayKeySound(pitch);
        pitch += 0.1f;
        yield return new WaitForSeconds(scoreAddTime);
        keys++;
        keysLeft--;
        score += 10;
        scoreAddTime /= timeModifier;
        scoreText.transform.DOScale(1.1f, scoreAddTime / 2).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            scoreText.transform.DOScale(1, scoreAddTime / 2).SetEase(Ease.OutExpo);
        });
        if (keysLeft > 0)
        {
            StartCoroutine(AddKeyScore());
        }
        else
        {
            scoreText.transform.DOScale(1.25f, 0.35f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                AudioManager.Instance.PlaySound("keyscollected", 1, 1);
                Instantiate(starBurst, new Vector3(1.5f, 0), Quaternion.identity);
                scoreText.transform.DOScale(1, 0.15f).SetEase(Ease.OutExpo);
            });
        }
    }
}
