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

    Rigidbody2D rigidbody;

    CircleCollider2D collider;

    Vector2 moveInput;

    Vector2 targetInput;

    Vector2 travelPoint;

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
                collider.enabled = true;
                Cinemachine.m_Follow = nextBubble;
                travelling = false;
                DOTween.To(() => targetLensSize, x => targetLensSize = x, 9f, .15f).SetEase(Ease.OutExpo);
            }
        }
    }

    public void TravelToBubble(Transform star, Transform bubble)
    {
        transform.position = star.position;
        Vector3 diff = transform.position - ((bubble.position - transform.position).normalized * 2);
        transform.DOMove(diff, LaunchAnticipationTime).SetEase(Ease.OutExpo);
        rigidbody.velocity = Vector3.zero;
        DOTween.To(() => targetLensSize, x => targetLensSize = x, 5, LaunchAnticipationTime).SetEase(Ease.OutExpo).
                OnComplete(() => { DOTween.To(() => targetLensSize, x => targetLensSize = x, 30, LaunchReleaseTime).SetEase(Ease.OutExpo); 
        Cinemachine.m_Follow = gameObject.transform;
        //Camera.main.transform.SetParent(transform);
        collider.enabled = false;
        nextBubble = bubble;
        travelPoint = bubble.position;
        travelling = true;
        rigidbody.velocity = (travelPoint - (Vector2)transform.position).normalized * TravelSpeed;
                });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canDash = true;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            shake = true;
            freezeTimer = FreezeTime;
        }
    }
}
