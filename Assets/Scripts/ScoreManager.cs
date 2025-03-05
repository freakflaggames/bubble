using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText, keyscoreText, planetscoreText;
    public GameObject scoreBonusPrefab;
    public Transform scoreBonusParent;
    public int score, keyscore, planetscore;
    public float smoothedScore, smoothSpeed;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        smoothedScore = Mathf.Lerp(smoothedScore, score, smoothSpeed);
        scoreText.text = Mathf.Round(smoothedScore) + "";
        keyscoreText.text = keyscore + "";
        planetscoreText.text = planetscore + "";
    }
    public void AddKeyScore()
    {
        score += 10;
        keyscore++;
    }
    public void AddPlanetScore()
    {
        GameObject scoreBonus = Instantiate(scoreBonusPrefab, scoreBonusParent);
        scoreBonus.transform.localScale = Vector3.zero;
        scoreBonus.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        score += 100;
        planetscore++;
    }
    public void AddGemScore(float delayTime)
    {
        TextMeshProUGUI scoreBonus = Instantiate(scoreBonusPrefab, scoreBonusParent).GetComponent<TextMeshProUGUI>();
        scoreBonus.transform.localScale = Vector3.zero;
        scoreBonus.transform.DOScale(0, delayTime).OnComplete(() => 
        {
            scoreBonus.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        });
        scoreBonus.color = new Color(.15f, .7f, 1, 1);
        scoreBonus.text = "+50";
        score += 50;
    }
    public void HideScoreBonus()
    {
        foreach(Transform child in scoreBonusParent)
        {
            Destroy(child.gameObject);
        }
    }
}
