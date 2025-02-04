using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    public CinemachineVirtualCamera Cinemachine;
    public LayerMask WallLayer;

    public int cageHits, cageHitsNeeded;
    public float targetLensSize = 9;
    public float StompSpeed;
    public float TravelSpeed;
    public float InputSmoothSpeed;
    public float LensSmoothSpeed;
    public float ScreenShakeFrequency;
    public float ScreenShakeTime;
    public float FreezeTime;
    public float LaunchAnticipationTime;
    public float LaunchReleaseTime;
    public float keypitch;
    public int worldsTraveled;
    public int keysCollected;
    float freezeTimer;
    float screenshaketimer;

    public Transform nextBubble;
    public GameObject starBurst;
    public GameObject cageBurst;

    public LineRenderer line;
    public ParticleSystem travelTrail;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer bgOverlay;

    public Sprite neutral, crouch, stretch;

    Rigidbody2D rigidbody;

    CircleCollider2D collider;

    Vector2 startMouseInput, mouseInput, moveInput;

    Vector2 targetInput;

    Vector2 travelPoint;

    bool isAboutToTravel;
    bool travelling;
    bool shake;
    public bool canDash;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        line.positionCount = 2;
    }
    private void Update()
    {
        RotateSprite();

        //TODO: move camera shit to camera manager
        if (freezeTimer > 0)
        {
            Time.timeScale = 0;
            freezeTimer -= Time.unscaledDeltaTime;
        }
        else
        {
            Time.timeScale = 1;
            if (shake)
            {
                screenshaketimer = ScreenShakeTime;
                DOTween.To(() => targetLensSize, x => targetLensSize = x, 8.5f, ScreenShakeTime/2).SetEase(Ease.OutExpo).
                OnComplete(() => { DOTween.To(() => targetLensSize, x => targetLensSize = x, 9, ScreenShakeTime/2).SetEase(Ease.OutExpo); });
                shake = false;
            }
        }
        if (screenshaketimer > 0)
        {
            Cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = ScreenShakeFrequency;
            screenshaketimer -= Time.unscaledDeltaTime;
        }
        else if (freezeTimer <= 0)
        {
            Cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        }

        Cinemachine.m_Lens.OrthographicSize = targetLensSize;

        travelTrail.enableEmission = travelling;

        //cant aim or perform dash while travelling to another world
        if (!travelling)
        {
            if (Input.GetMouseButtonDown(0) && canDash && !isAboutToTravel)
            {
                startMouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                AudioManager.Instance.PlayWindupSound();
            }
            //begin dash aim when click is held
            if (Input.GetMouseButton(0) && canDash && !isAboutToTravel)
            {
                AimDash();
            }
            //dash performed when click is released
            if (Input.GetMouseButtonUp(0) && canDash)
            {
                Dash();
            }
        }
        else
        {
            //if close enough to target bubble, land on it
            if (Vector3.Distance(travelPoint, transform.position) < 1f)
            {
                if (travelling)
                {
                    LandOnBubble();
                }
            }
        }
    }
    public void RotateSprite()
    {
        if (!isAboutToTravel)
        {
            //rotate player sprite towards velocity
            Vector3 diff = rigidbody.velocity.normalized;
            float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, deg + 270);
        }
        else
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void AimDash()
    {
        mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        moveInput = (mouseInput - startMouseInput).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -moveInput, 100, WallLayer);

        if (cageHits >= cageHitsNeeded)
        {
            DOTween.To(() => targetLensSize, x => targetLensSize = x, 8, LaunchAnticipationTime).SetEase(Ease.OutExpo);
        }
        spriteRenderer.sprite = crouch;
        Time.timeScale = 0.1f;
        if (hit.collider != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hit.point);
            float length = Vector2.Distance(transform.position, hit.point);
            float width = line.startWidth;
            line.material.mainTextureScale = new Vector2(length / width, 1.0f);
        }
        else
        {
            Debug.DrawRay(transform.position, moveInput, Color.red);
        }
    }
    public void Dash()
    {
        if (cageHits >= cageHitsNeeded)
        {
            DOTween.To(() => targetLensSize, x => targetLensSize = x, 9, LaunchAnticipationTime).SetEase(Ease.OutExpo);
        }
        AudioManager.Instance.StopWindupSound();
        AudioManager.Instance.PlaySound("woosh", 0.9f, 1);
        spriteRenderer.transform.DOScale(new Vector2(.75f, 1.5f), 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            spriteRenderer.transform.DOScale(1, 0.1f).SetEase(Ease.OutExpo);
        });
        spriteRenderer.sprite = stretch;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        Time.timeScale = 1;
        rigidbody.velocity = -moveInput * StompSpeed;
    }
    public void TravelToBubble(Transform star, Transform bubble)
    {
        //TODO: AHHHHHHHHHHHHHHHH
        Cinemachine.m_Follow = nextBubble.transform.GetChild(0);
        spriteRenderer.gameObject.SetActive(false);
        HandManager.Instance.StopGrabAnimation();
        HandManager.Instance.ResetTimer();
        keypitch = 1;
        spriteRenderer.sprite = crouch;
        isAboutToTravel = true;
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        collider.enabled = false;
        transform.position = star.position;
        rigidbody.velocity = Vector3.zero;

        DOTween.To(() => targetLensSize, x => targetLensSize = x, 5, LaunchAnticipationTime).SetEase(Ease.OutExpo).
                OnComplete(() => { DOTween.To(() => targetLensSize, x => targetLensSize = x, 30, LaunchReleaseTime).SetEase(Ease.OutExpo); 
        Cinemachine.m_Follow = gameObject.transform;
        AudioManager.Instance.PlaySound("cannon", 1, 1);
                    spriteRenderer.gameObject.SetActive(true);
                    nextBubble = bubble;
        travelPoint = bubble.position;
        isAboutToTravel = false;
        travelling = true;
                    AudioManager.Instance.PlayHappySound();
                    spriteRenderer.sprite = stretch;
        rigidbody.velocity = (travelPoint - (Vector2)transform.position).normalized * TravelSpeed;
                    ScoreManager.Instance.AddPlanetScore();
                });
    }
    public void LandOnBubble()
    {
        worldsTraveled++;
        HandManager.Instance.StartTimer();
        spriteRenderer.sprite = neutral;
        rigidbody.velocity /= 10;
        canDash = true;
        collider.enabled = true;
        Cinemachine.m_Follow = nextBubble;
        travelling = false;
        DOTween.To(() => targetLensSize, x => targetLensSize = x, 9f, .15f).SetEase(Ease.OutExpo);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: THIS SUCKS
        spriteRenderer.transform.DOScale(new Vector2(1.5f, .75f), 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.1f).SetEase(Ease.OutExpo);
            });
        spriteRenderer.sprite = neutral;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AudioManager.Instance.PlayHitSound();
            AudioManager.Instance.PlaySound("dodgeball", 0.8f, 1.1f);
            shake = true;
            freezeTimer = FreezeTime;
        }
        else
        {
            canDash = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySound("bounce", 0.8f, 1.1f);
        }
        if (collision.gameObject.CompareTag("Cage"))
        {
            Cinemachine.m_Follow = transform;
            cageHits++;
            if (cageHits < cageHitsNeeded)
            {
                float lens = Mathf.Lerp(9, 5, (float)cageHits / cageHitsNeeded);
                DOTween.To(() => targetLensSize, x => targetLensSize = x, lens, 0.1f).SetEase(Ease.OutExpo);
                AudioManager.Instance.PlaySound("metalhit", 0.8f, 1.1f);
                AudioManager.Instance.PlayHitSound();
                collision.gameObject.transform.DORotate(new Vector3(0, 0, Random.Range(-15, 15)), 0.1f);
                collision.gameObject.transform.DOScale(1.1f, 0.1f).OnComplete(() => { collision.gameObject.transform.DOScale(1f, 0.1f); });
            }
            else
            {
                Time.timeScale = 0.1f;
                AudioManager.Instance.PlayMusic();
                Instantiate(cageBurst, transform.position, Quaternion.identity);
                DOTween.To(() => targetLensSize, x => targetLensSize = x, 9, 1).SetEase(Ease.OutExpo);
                AudioManager.Instance.PlaySound("break", 1, 1);
                Cinemachine.m_Follow = nextBubble;
                Destroy(collision.gameObject);
            }
            rigidbody.velocity = Vector3.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            Instantiate(starBurst, transform.position, Quaternion.identity);
            ScoreManager.Instance.AddKeyScore();
            keysCollected++;
            AudioManager.Instance.PlayKeySound(keypitch);
            keypitch += 0.1f;
            Destroy(collision.gameObject);
        }
    }
}
