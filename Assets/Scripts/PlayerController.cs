using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public CinemachineVirtualCamera Cinemachine;
    public LayerMask WallLayer;

    public int cageHits, cageHitsNeeded;
    public float targetLensSize = 9;
    public float StompSpeed;
    public float TravelSpeed;
    public float HitForce;
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
    public GameObject gemBurst;

    public GameObject gemTrailPrefab;
    public List<GameObject> collectedGems;

    public LineRenderer line;
    public ParticleSystem travelTrail;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer bgOverlay;

    public Sprite neutral, crouch, stretch, hurt;

    Rigidbody2D rigidbody;

    CircleCollider2D collider;

    Vector2 startMouseInput, mouseInput, moveInput;
    public Image startInputGraphic, currentInputGraphic;

    Vector2 targetInput;

    Vector2 travelPoint;
    Vector2 travelVelocity;

    bool isAboutToTravel;
    bool travelling;
    bool shake;
    bool freezeReturn;
    public bool canDash;

    bool firstLevel;
    bool firstDrag;
    bool firstKey;
    bool firstCannon;

    public int slimeJumps;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        line.positionCount = 2;
    }
    private void Start()
    {
        collider.enabled = false;
        travelVelocity = (nextBubble.position - transform.position).normalized * TravelSpeed;
        travelling = true;
    }
    private void Update()
    {
        RotateSprite();

        //TODO: move camera shit to camera manager
        if (freezeTimer > 0 && !PauseMenu.Instance.paused)
        {
            Time.timeScale = 0;
            freezeTimer -= Time.unscaledDeltaTime;
            freezeReturn = false;
        }
        else
        {
            if (!freezeReturn && !PauseMenu.Instance.paused)
            {
                Time.timeScale = 1;
                freezeReturn = true;
            }
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
        if (!travelling && !isAboutToTravel)
        {
            if (Input.GetMouseButtonDown(0) && canDash)
            {
                startMouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                AudioManager.Instance.PlayWindupSound();
            }
            //begin dash aim when click is held
            if (Input.GetMouseButton(0) && canDash)
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
            if (Vector3.Distance(travelPoint, transform.position) < 1)
            {
                if (travelling)
                {
                    LandOnBubble();
                }
            }
        }

        if (travelling)
        {
            rigidbody.velocity = travelVelocity;
        }

        for (int i = 0; i < collectedGems.Count; i++)
        {
            if (gameObject != null && collectedGems[i] != null)
            {
                float distanceInterval = 2;
                float smoothTime = 0.25f;
                Vector3 diff = collectedGems[i].transform.position - transform.position;
                Vector3 targetPos = diff.normalized + transform.position;
                collectedGems[i].GetComponent<Rigidbody2D>().MovePosition(Vector3.Lerp(collectedGems[i].transform.position, targetPos, smoothTime * (i+1)));
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
        //dash direction is calculated from where you started your touch vs where it is now
        mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //move input is inverted so it feels like youre pulling back to launch
        moveInput = -(mouseInput - startMouseInput).normalized;

        startInputGraphic.gameObject.SetActive(true);
        startInputGraphic.transform.position = Camera.main.WorldToScreenPoint(startMouseInput);

        currentInputGraphic.gameObject.SetActive(true);
        currentInputGraphic.transform.position = Camera.main.WorldToScreenPoint(mouseInput);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput, 100, WallLayer);

        if (cageHits >= cageHitsNeeded)
        {
            DOTween.To(() => targetLensSize, x => targetLensSize = x, 8, LaunchAnticipationTime).SetEase(Ease.OutExpo);
        }

        //could be using a state machine for changing sprites 
        spriteRenderer.sprite = crouch;
        Time.timeScale = 0.05f;
        if (hit.collider != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hit.point);
            float length = Vector2.Distance(transform.position, hit.point);
            float width = line.startWidth;
            line.material.mainTextureScale = new Vector2(length / width, 1.0f);
        }

        if (!firstDrag)
        {
            Tutorial.Instance.NextSection();
            firstDrag = true;
        }
    }
    public void Dash()
    {
        startInputGraphic.gameObject.SetActive(false);
        currentInputGraphic.gameObject.SetActive(false);

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
        if (Time.timeScale > 0)
        {
            Time.timeScale = 1;
        }
        if (slimeJumps > 0)
        {
            transform.parent.parent.DOScale(7.5f, 0.1f).SetEase(Ease.OutBack).OnComplete(() => { transform.parent.parent.DOScale(6.5f, 0.1f).SetEase(Ease.OutBack); });
            AudioManager.Instance.PlaySound("slimeenter", 0.9f, 1.1f);
            slimeJumps--;
        }
        else
        {
            UnAnchorPlayer();
            rigidbody.velocity = moveInput * StompSpeed;
        }
    }
    public void TravelToBubble(Transform star, Transform bubble)
    {
        if (!firstCannon)
        {
            Tutorial.Instance.NextSection();
            firstCannon = true;
        }
        //TODO: AHHHHHHHHHHHHHHHH
        Time.timeScale = 1;
        ResetLineRenderer();
        ScoreManager.Instance.AddPlanetScore();
        Cinemachine.m_Follow = nextBubble.transform.GetChild(0);
        spriteRenderer.gameObject.SetActive(false);
        HandManager.Instance.StopGrabAnimation();
        HandManager.Instance.ResetTimer();
        keypitch = 1;
        spriteRenderer.sprite = crouch;
        isAboutToTravel = true;
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        collider.enabled = false;
        if (star != null)
        {
            transform.position = star.position;
        }
        rigidbody.velocity = Vector3.zero;

        DOTween.To(() => targetLensSize, x => targetLensSize = x, 5, LaunchAnticipationTime).SetEase(Ease.OutExpo).
                OnComplete(() => { DOTween.To(() => targetLensSize, x => targetLensSize = x, 30, LaunchReleaseTime).SetEase(Ease.OutExpo); 
        Cinemachine.m_Follow = gameObject.transform;
        AudioManager.Instance.PlaySound("cannon", 1, 1);
                    spriteRenderer.gameObject.SetActive(true);
                    nextBubble = bubble;
        travelPoint = bubble.position;
        isAboutToTravel = false;
                    AudioManager.Instance.PlayHappySound();
                    spriteRenderer.sprite = stretch;
                    travelVelocity = (travelPoint - (Vector2)transform.position).normalized * TravelSpeed;
                    travelling = true;
                    ScoreManager.Instance.HideScoreBonus();
                });
    }
    public void LandOnBubble()
    {
        if (!firstLevel)
        {
            Tutorial.Instance.NextSection();
            firstLevel = true;
        }
        else
        {
            HandManager.Instance.StartTimer();
        }
        worldsTraveled++;
        spriteRenderer.sprite = neutral;
        rigidbody.velocity /= 10;
        canDash = true;
        collider.enabled = true;
        Cinemachine.m_Follow = nextBubble;
        travelling = false;
        DOTween.To(() => targetLensSize, x => targetLensSize = x, 9f, .15f).SetEase(Ease.OutExpo);
    }
    public void ResetLineRenderer()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
    }
    public void ResetCollectedGems()
    {
        for (int i = 0; i < collectedGems.Count; i++)
        {
            Destroy(collectedGems[i]);
        }
    }
    public void HitByEnemy(Transform enemy)
    {
        UnAnchorPlayer();
        if (collectedGems.Count > 0)
        {
            AudioManager.Instance.PlaySound("poof", 0.9f, 1.1f);
            Instantiate(gemBurst, collectedGems[collectedGems.Count - 1].transform.position, Quaternion.identity);
            Destroy(collectedGems[0]);
            collectedGems.RemoveAt(0);
        }
        rigidbody.velocity = (transform.position - enemy.transform.position) * HitForce;
        AudioManager.Instance.PlayHitSound();
        AudioManager.Instance.PlaySound("dodgeball", 0.8f, 1.1f);
        spriteRenderer.sprite = hurt;
        shake = true;
        freezeTimer = FreezeTime;
    }
    public void UnAnchorPlayer()
    {
        transform.SetParent(null);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: THIS SUCKS
        //this stuff should all be moved to their respective scripts
        spriteRenderer.transform.DOScale(new Vector2(1.5f, .75f), 0.1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.1f).SetEase(Ease.OutExpo);
            });
        if (spriteRenderer.sprite != hurt)
        {
            spriteRenderer.sprite = neutral;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitByEnemy(collision.transform);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySound("bounce", 0.8f, 1.1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            if (!firstKey)
            {
                Tutorial.Instance.NextSection();
                firstKey = true;
            }
            Instantiate(starBurst, collision.gameObject.transform.position, Quaternion.identity);
            keysCollected++;
            AudioManager.Instance.PlayKeySound(keypitch);
            keypitch += 0.1f;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Gem")) 
        {
            Instantiate(gemBurst, transform.position, Quaternion.identity);
            GameObject gem = Instantiate(gemTrailPrefab, transform.position, Quaternion.identity);
            gem.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutBack).OnComplete(() => { gem.transform.DOScale(1, 0.1f); });
            gem.GetComponent<SpriteRenderer>().sprite = collision.GetComponent<SpriteRenderer>().sprite;
            collectedGems.Add(gem);
            Destroy(collision.gameObject);
            float pitch = 1 + (0.25f * collectedGems.Count);
            AudioManager.Instance.PlaySound("gempickup",pitch,pitch);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitByEnemy(collision.gameObject.transform);
        }
    }
}
