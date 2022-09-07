using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public static AudioManager Instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip walkAudio;

    /*private void Awake()
    {
        Instance = this;
    }*/

    private void OnEnable()
    {
        EventManager.Register<AudioManager>(WalkAudio);
    }

    private void OnDisable()
    {
        EventManager.Register<AudioManager>(StopAudio);
    }

    public void WalkAudio(AudioManager audio)
    {
        if(audioSource == null || walkAudio == null)
        {
            return;
        }

        audioSource.clip = walkAudio;
        audioSource.Play();
    }

    public void StopAudio(AudioManager audio)
    {
        if(audioSource == null)
        {
            return;
        }

        audioSource.Stop();
    }
}
