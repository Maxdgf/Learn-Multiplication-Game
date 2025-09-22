/*
======================================================
| Simple audio player module for AudioSource object. |
======================================================
*/

using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource; //AudioSource main object for module functions.

    //Simple AudioClip playback function.
    //----------------------------------------------------------
    //1) setting AudioClip to AudioSource object (if not setted)
    //2) starting playback
    public void PlayAudio(AudioClip audioClip)
    {
        if (!IsCurrentAudioClipAlreadySetted(audioClip))
        {
            audioSource.clip = audioClip; //1)
        }

        audioSource.Play(); //2)
    }

    //AudioClip playback after delay (coroutine).
    //----------------------------------------------------------
    //1) coroutine delay
    //2) setting AudioClip to AudioSource object (if not setted)
    //3) starting playback
    public IEnumerator PlayAudioAfterDelay(AudioClip audioClip, float delay)
    {
        yield return new WaitForSeconds(delay); //1)

        if (!IsCurrentAudioClipAlreadySetted(audioClip))
        {
            audioSource.clip = audioClip; //2)
        }

        audioSource.Play(); //3)
    }

    //Checking, is AudioClip already setted (returning true or false).
    //-----------------------------------------------------------------------
    //1) getting audioSource AudioClip and match this with incoming AudioClip
    private bool IsCurrentAudioClipAlreadySetted(AudioClip currentClip)
    {
        if (audioSource.clip == currentClip) //1)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }
}
