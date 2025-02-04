using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText, keyscoreText, planetscoreText;
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
        score += 100;
        planetscore++;
    }
}
