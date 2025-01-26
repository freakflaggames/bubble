using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class HandManager : MonoBehaviour
{
    public Transform hand;
    public float timer, timerMax, timeDecay;
    public bool active;
    public static HandManager Instance;
    public TextMeshProUGUI timerText;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        timerText.text = Mathf.Round(timer) + "";
        hand.transform.localRotation = Quaternion.Euler(Mathf.Lerp(-57, -47, timer / timerMax), hand.transform.rotation.y, hand.transform.rotation.z);
        if (active)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            if (active)
            {
                int worlds = FindAnyObjectByType<PlayerController>().worldsTraveled;
                int keys = FindAnyObjectByType<PlayerController>().keysCollected;
                PlayerPrefs.SetInt("worlds", worlds);
                PlayerPrefs.SetInt("keys", keys);
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlaySound("pop", 1, 1);
                FindAnyObjectByType<PlayerController>().transform.position = Camera.main.transform.position;
                FindAnyObjectByType<PlayerController>().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Destroy(FindAnyObjectByType<PlayerController>().nextBubble.gameObject);
                FindAnyObjectByType<PlayerController>().canDash = false;
                active = false;
                StartCoroutine(WaitToSceneChange());
            }
        }
    }
    IEnumerator WaitToSceneChange()
    {
        yield return new WaitForSeconds(3);
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
