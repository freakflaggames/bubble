using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Speed_timelineCtrl : MonoBehaviour
{
    public PlayableDirector director;
    public float newSpeed = 2f;
    
    
    void Start()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(newSpeed);
    }

}
