using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    public CinemachineVirtualCamera Cinemachine;
    public LayerMask WallLayer;

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
    float freezeTimer;
    float screenshaketimer;

    Transform nextBubble;

    public LineRenderer line;
    public ParticleSystem travelTrail;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer bgOverlay;

    public Sprite neutral, crouch, stretch;

    Rigidbody2D rigidbody;

    CircleCollider2D collider;

    Vector2 moveInput;

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
        if (!isAboutToTravel)
        {
            Vector3 diff = rigidbody.velocity.normalized;
            float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, deg + 270);
        }
        else
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

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

        if (!travelling)
        {
            Vector2 mouseInput = (Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position).normalized;

            moveInput = mouseInput;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput, 100, WallLayer);

            if (Input.GetMouseButton(0) && canDash)
            {
                DOTween.To(() => targetLensSize, x => targetLensSize = x, 8, LaunchAnticipationTime).SetEase(Ease.OutExpo);
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

            if (Input.GetMouseButtonUp(0) && canDash)
            {
                DOTween.To(() => targetLensSize, x => targetLensSize = x, 9, LaunchAnticipationTime).SetEase(Ease.OutExpo);
                spriteRenderer.sprite = stretch;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position);
                Time.timeScale = 1;
                rigidbody.velocity = moveInput * StompSpeed;
                canDash = false;
            }
        }
        else
        {
            if (Vector3.Distance(travelPoint, transform.position) < 1f)
            {
                if (travelling)
                {
                    spriteRenderer.sprite = neutral;
                    rigidbody.velocity /= 10;
                    canDash = true;
                }
                collider.enabled = true;
                Cinemachine.m_Follow = nextBubble;
                travelling = false;
                DOTween.To(() => targetLensSize, x => targetLensSize = x, 9f, .15f).SetEase(Ease.OutExpo);
            }
        }
    }

    public void TravelToBubble(Transform star, Transform bubble)
    {
        spriteRenderer.sprite = crouch;
        isAboutToTravel = true;
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        collider.enabled = false;
        transform.position = star.position;
        rigidbody.velocity = Vector3.zero;
        DOTween.To(() => targetLensSize, x => targetLensSize = x, 5, LaunchAnticipationTime).SetEase(Ease.OutExpo).
                OnComplete(() => { DOTween.To(() => targetLensSize, x => targetLensSize = x, 30, LaunchReleaseTime).SetEase(Ease.OutExpo); 
        Cinemachine.m_Follow = gameObject.transform;
        //Camera.main.transform.SetParent(transform);
        nextBubble = bubble;
        travelPoint = bubble.position;
        isAboutToTravel = false;
        travelling = true;
                    spriteRenderer.sprite = stretch;
        rigidbody.velocity = (travelPoint - (Vector2)transform.position).normalized * TravelSpeed;
                });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        spriteRenderer.transform.DOScale(new Vector2(1.25f, .75f), 0.05f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1, 0.05f).SetEase(Ease.OutExpo);
            });
        spriteRenderer.sprite = neutral;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            shake = true;
            freezeTimer = FreezeTime;
        }
        else
        {
            canDash = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
        }
    }
}
