/*
============================
| Animation manager module |
============================
*/

using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    //animation playback function
    public void PlayAnimation(Animator animator, string animationName, bool isOneCall)
    {
        //matching animation call state
        switch (isOneCall)
        {
            case true:
                animator.Play(animationName); //one animation call
                break;
            case false:
                animator.Play(animationName, 0, -1f); //resetting animator state for next animation playback
                break;
        }
    }

    //animation playback after delay (coroutine)
    public IEnumerator PlayAnimationAfterDelay(Animator animator, string animationName, float delay, bool isOneCall)
    {
        yield return new WaitForSeconds(delay); //delay

        //matching animation call state
        switch (isOneCall)
        {
            case true:
                animator.Play(animationName); //one animation call
                break;
            case false:
                animator.Play(animationName, 0, -1f); //resetting animator state for next animation playback
                break;
        }
    }
}
