using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class GameOver : MonoBehaviour
{
    public GameObject starBurst;
    public float scoreAddTime;
    public float pitch;
    public int score;
    int _score;
    public TextMeshProUGUI worldScore, keyScore, scoreText, hiscoreText;
    private void Start()
    {
        hiscoreText.text = PlayerPrefs.GetInt("highscore") + "";
        score = PlayerPrefs.GetInt("score");
        StartCoroutine(WaitToAddScore());
    }
    private void Update()
    {
        scoreText.text = _score + "";
    }
    IEnumerator WaitToAddScore()
    {
        yield return new WaitForSeconds(0);
        StartCoroutine(AddScore());
    }
    IEnumerator AddScore()
    {
        DOTween.To(() => _score, x => _score = x, score, scoreAddTime).SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(scoreAddTime);
        AudioManager.Instance.PlaySound("keyscollected", 1, 1);
        scoreText.transform.DOScale(1.25f, 0.35f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Instantiate(starBurst, new Vector3(-2, 0), Quaternion.identity);
            scoreText.transform.DOScale(1, 0.15f).SetEase(Ease.OutExpo);
            StartCoroutine(WaitToCheckBestScore());
        });
    }

    IEnumerator WaitToCheckBestScore()
    {
        yield return new WaitForSeconds(0.5f);
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            AudioManager.Instance.PlaySound("keyscollected", 2, 2);
            PlayerPrefs.SetInt("highscore", score);
            hiscoreText.text = PlayerPrefs.GetInt("highscore") + "";
            hiscoreText.transform.DOScale(1.25f, 0.35f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                Instantiate(starBurst, new Vector3(3.5f, 0), Quaternion.identity);
                hiscoreText.transform.DOScale(1, 0.15f).SetEase(Ease.OutExpo);
            });
        }
    }
}
