using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
public class HandManager : MonoBehaviour
{
    //TODO: move this to a timermanager or something
    public Transform endTransition;
    public SpriteRenderer endBlack;
    public Animator animator;
    public float timer, timerMax, timeDecay;
    public bool active;
    public bool grabbing;
    public static HandManager Instance;
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
        if (active)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 2.1f)
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
                Destroy(timerText.transform.parent.gameObject);
                int worlds = FindAnyObjectByType<PlayerController>().worldsTraveled;
                int keys = FindAnyObjectByType<PlayerController>().keysCollected;
                PlayerPrefs.SetInt("worlds", worlds);
                PlayerPrefs.SetInt("keys", keys);
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlaySound("pop", 1, 1);
                AudioManager.Instance.PlaySound("choke", 1, 1);
                FindAnyObjectByType<PlayerController>().transform.localScale = Vector3.zero;
                FindAnyObjectByType<PlayerController>().transform.position = Camera.main.transform.position;
                FindAnyObjectByType<PlayerController>().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Destroy(FindAnyObjectByType<PlayerController>().nextBubble.gameObject);
                FindAnyObjectByType<PlayerController>().canDash = false;
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
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOver");
    }
    public void StartTimer()
    {
        active = true;
    }
    public void ResetTimer()
    {
        timerMax -= timeDecay;
        if (timerMax < 2)
        {
            timerMax = 2;
        }
        timer = timerMax;
        active = false;
    }
}
