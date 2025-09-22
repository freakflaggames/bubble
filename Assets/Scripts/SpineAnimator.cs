using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAnimator : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset pop, move;
    public float Mixduration = 1.5f;

    void Start()
    {
        // Check if the skeletonAnimation component is assigned
        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation is not assigned!");
            return;
        }

        // Get the AnimationState from the SkeletonAnimation component
        var state = skeletonAnimation.AnimationState;

        // Set the first animation on track 0. It will play once (loop: false).
        state.SetAnimation(0, pop, false);

        // Add the second animation to track 0. It will automatically queue after the first one.
        // It is set to loop indefinitely (loop: true).
        // The last parameter (0) is a delay, which means it will start immediately after the first one finishes.
        // This is equivalent to calling AddAnimation(0, "looping_animation", true, 0).
        
        state.AddAnimation(0, move, true, Mixduration);
    }
}
