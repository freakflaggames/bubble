using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class HandManager : MonoBehaviour
{
    //TODO: move this to a timermanager or something
    public Transform goddess;
    public Transform endTransition;
    public Transform UIparent;
    public SpriteRenderer endBlack;
    public Animator animator;
    public float timer, timerMax, timeDecay;
    public bool active;
    bool flashed;
    public bool grabbing;
    public static HandManager Instance;
    public Image timerFill, timerDangerOverlay;
    public TextMeshProUGUI timerText;
    private void Awake()
    {
        Instance = this;
    }
    public void StopGrabAnimation()
    {
        animator.SetTrigger("return");
        grabbing = false;
    }
    private void Update()
    {
        timerText.text = Mathf.Round(timer) + "";
        timerFill.fillAmount = timer / timerMax;
        if (active)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 3 && !flashed && active)
        {
            flashed = true;
            timerDangerOverlay.DOColor(new Color(1, 0, 0, 0.5f), 0.5f / 2).OnComplete(() => {
                timerDangerOverlay.DOColor(new Color(1, 0, 0, 0f), 0.5f / 2).OnComplete(() =>
                {
                    flashed = false;
                });
            });
        }
        if (timer <= 1.1F)
        {
            if (!grabbing)
            {
                animator.SetTrigger("grab");
                grabbing = true;
            }
        }
        if (timer <= 0)
        {
            if (active)
            {
                Time.timeScale = 1;
                UIparent.gameObject.SetActive(false);
                PlayerController Player = FindAnyObjectByType<PlayerController>();
                int worlds = Player.worldsTraveled;
                int keys = Player.keysCollected;
                PlayerPrefs.SetInt("score", ScoreManager.Instance.score);
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlaySound("pop", 1, 1);
                AudioManager.Instance.PlaySound("choke", 1, 1);
                Player.transform.localScale = Vector3.zero;
                Player.transform.position = Camera.main.transform.position;
                Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Player.ResetLineRenderer();
                Player.ResetCollectedGems();
                Destroy(Player.nextBubble.gameObject);
                Player.canDash = false;
                active = false;
                StartCoroutine(WaitToSceneChange());
                endTransition.transform.DOScale(0.65f, 3);
            }
        }
    }
    IEnumerator WaitToSceneChange()
    {
        yield return new WaitForSeconds(2);
        AudioManager.Instance.PlaySound("slide", 1, 1);
        yield return new WaitForSeconds(1.5f);
        endBlack.DOColor(new Color(0, 0, 0, 1),.5f);
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("GameOver");
    }
    public void StartTimer()
    {
        active = true;
    }
    public void ResetTimer()
    {
        timerMax -= timeDecay;
        if (timerMax < 1)
        {
            timerMax = 1;
        }
        timer = timerMax;
        active = false;
    }
}
