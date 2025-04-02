using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class GameOver : MonoBehaviour
{
    public int currentScore;
    float smoothedScore;
    int highestIndex;
    public GameObject starBurst;
    public GameObject inputOverlay;
    public GameObject leaderboardBar;
    public Transform leaderboardParent;
    public TextMeshProUGUI scoreText;
    public TMP_InputField nameInput;
    public Image blackOverlay;
    public bool clear;
    private void Start()
    {
        currentScore = PlayerPrefs.GetInt("score");
        highestIndex = PlayerPrefs.GetInt("highestIndex");
        if (clear)
        {
            ClearScores();
        }
        blackOverlay.DOColor(new Color(0, 0, 0, 1), .5f).OnComplete(()=> { blackOverlay.DOColor(new Color(0, 0, 0, 0), .5f); });
    }
    private void Update()
    {
        scoreText.text = Mathf.Round(smoothedScore) + "";
    }
    
    public void ClearScores()
    {
        highestIndex = 0;
        PlayerPrefs.SetInt("highestIndex", highestIndex);
    }
    public void CloseOverlay()
    {
        inputOverlay.gameObject.SetActive(false);
        SortLeaderboardScores();
    }

    public void SubmitScore()
    {
        string name = nameInput.text;
        PlayerPrefs.SetInt(name, currentScore);
        PlayerPrefs.SetString(highestIndex+"", name);
        PlayerPrefs.SetInt("highestIndex", highestIndex + 1);
        highestIndex++;
        CloseOverlay();
    }
    public void SortLeaderboardScores()
    {
        var scores = new List<KeyValuePair<string,int>>();
        for (int i =0; i < highestIndex; i++)
        {
            string name = PlayerPrefs.GetString(i + "");
            int score = PlayerPrefs.GetInt(name);
            scores.Add(new KeyValuePair<string, int>(name, score));
        }
        scores.Sort((x, y) => y.Value.CompareTo(x.Value));
        for (int i = 0; i < scores.Count; i++)
        {
            string name = scores[i].Key;
            PlayerPrefs.SetString(i + "", name);
        }
        GenerateLeaderboardBars();
    }
    public void GenerateLeaderboardBars()
    {
        for (int i = 0; i < highestIndex; i++)
        {
            string name = PlayerPrefs.GetString(i + "");
            int score = PlayerPrefs.GetInt(name);
            GenerateLeaderboardBar(i + 1, name, score);
        }
        RevealScore();
    }
    public void RevealScore()
    {
        scoreText.transform.DOScale(1.5f, 1).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            scoreText.transform.DOScale(1, 0.25f).SetEase(Ease.OutExpo);
            Vector3 particlePosition = new Vector3(-4.5f, 0);
            Instantiate(starBurst, particlePosition, Quaternion.identity);
            AudioManager.Instance.PlaySound("keyscollected",1,1);
        });
        DOTween.To(() => smoothedScore, x => smoothedScore = x, currentScore, 1).SetEase(Ease.OutExpo);
    }
    public void GenerateLeaderboardBar(int index, string name, int score)
    {
        LeaderboardBar bar = Instantiate(leaderboardBar, leaderboardParent).GetComponent<LeaderboardBar>();
        bar.index.text = index + "";
        bar.name.text = name;
        bar.score.text = score + "";
    }
}
