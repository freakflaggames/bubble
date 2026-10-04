using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Speed_timelineCtrl : MonoBehaviour
{
    public PlayableDirector director;

    [Header("Speed Setting")]
    [Tooltip("Speed of the timeline to ease toward")]

    public float newSpeed = 1.5f;

    [Tooltip("Accelarated time to reach the actual speed-up speed")]
    public float acceleration = 5f;

    float currentSpeed = 1f;

    void Start()
    {
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }

        //play the current speed to match what the graph is
        if (director != null && director.playableGraph.IsValid())
        {
            currentSpeed = (float)director.playableGraph.GetRootPlayable(0).GetSpeed();
        }
    }

    private void Update()
    {
        if (director == null || !director.playableGraph.IsValid()) return;

        //smooth interpolate from the current speed to target speed
        currentSpeed = Mathf.MoveTowards(currentSpeed, newSpeed, acceleration * Time.deltaTime);

        // appy the dampened to the timeline
        director.playableGraph.GetRootPlayable(0).SetSpeed(currentSpeed);
    }

}
