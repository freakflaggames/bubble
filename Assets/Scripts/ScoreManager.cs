using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    PlayerController player;
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText, baseScoreBonus, timeScoreBonus, gemScoreBonus, keyScoreBonus;
    public Image gemScoreIcon;
    public Transform scoreBonusParent;
    public int score;
    public float smoothedScore, smoothSpeed, scoringTime, scoreBonusTime;
    private void Awake()
    {
        Instance = this;
        player = FindAnyObjectByType<PlayerController>().gameObject.GetComponent<PlayerController>();
    }
    private void Update()
    {
        smoothedScore = Mathf.Lerp(smoothedScore, score, smoothSpeed);
        scoreText.text = Mathf.Round(smoothedScore) + "";
    }
    public void AddPlanetScore()
    {

        Transform scoreBonus = baseScoreBonus.transform.parent;
        scoreBonus.gameObject.SetActive(true);
        scoreBonus.transform.localScale = Vector3.zero;
        baseScoreBonus.text = "+100";
        score += 100;

        int keyscore = player.keysCollected * 10;
        int gemscore = player.collectedGems.Count * 50;

        int scoreBonusCount = 1 + (keyscore > 0 ? 1 : 0) + (gemscore > 0 ? 1 : 0);
        scoreBonusTime = scoringTime / scoreBonusCount;

        if (player.collectedGems.Count > 0)
        {
            gemScoreIcon.sprite = player.collectedGems[0].GetComponent<SpriteRenderer>().sprite;
        }
        for (int i = 0; i < player.collectedGems.Count; i++)
        {
            Destroy(player.collectedGems[i]);
        }

        AudioManager.Instance.PlaySound("bounce", 1, 1);
        scoreBonus.transform.DOScale(1, scoreBonusTime/2).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                StartCoroutine(KeyScore(player.keysCollected, 0, scoreBonusTime / player.keysCollected/2));
            });
    }
    public IEnumerator KeyScore(int numKeysToScore, int _score, float time)
    {
        if (numKeysToScore > 0)
        {
            Transform keyScore = keyScoreBonus.transform.parent;
            keyScore.gameObject.SetActive(true);

            if (_score == 0)
            {
                keyScore.transform.localScale = Vector3.zero;
            }

            float keyPitch = 1 + (0.1f * (player.keysCollected - numKeysToScore));
            print(keyPitch);
            AudioManager.Instance.PlayKeySound(keyPitch);
            keyScore.transform.DOScale(1.1f, time / 2).SetEase(Ease.OutBack).OnComplete(() => {
                if (_score > 0)
                { keyScore.transform.DOScale(1, time / 2).SetEase(Ease.OutBack); }
            });

            _score += 10;
            score += 10;
            keyScoreBonus.text = "+" + _score;
            yield return new WaitForSeconds(time);
            StartCoroutine(KeyScore(numKeysToScore - 1, _score, time));
        }
        else
        {
            player.keysCollected = 0;
            StartCoroutine(GemScore(player.collectedGems.Count, 0, scoreBonusTime / player.collectedGems.Count * 2));
        }
    }
    public IEnumerator GemScore(int numGemsToScore, int _score, float time)
    {
        
        if (numGemsToScore > 0)
        {
            Transform gemScore = gemScoreBonus.transform.parent;
            gemScore.gameObject.SetActive(true);

            if (_score == 0)
            {
                gemScore.transform.localScale = Vector3.zero;
            }

            float pitch = 1 + (0.1f * (player.collectedGems.Count - numGemsToScore));
            AudioManager.Instance.PlaySound("crunch", pitch, pitch);
            gemScore.transform.DOScale(1.1f + 0.1f * player.collectedGems.Count, time / 2).SetEase(Ease.OutBack).OnComplete(() =>
            {
                if (_score > 0)
                { gemScore.transform.DOScale(1, time / 2).SetEase(Ease.OutBack); }
            });

            _score += 50;
            score += 50;
            gemScoreBonus.text = "+" + _score;
            yield return new WaitForSeconds(time);
            StartCoroutine(GemScore(numGemsToScore - 1, _score, time));
        }
        else
        {
            player.collectedGems.Clear();
        }
    }
    public void HideScoreBonus()
    {
        foreach(Transform child in scoreBonusParent)
        {
            child.gameObject.SetActive(false);
        }
    }
}
