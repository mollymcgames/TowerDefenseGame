using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    public AudioClip backgroundMusic; // The background music

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Don't destroy this object when loading a new scene
            audioSource = GetComponent<AudioSource>();

            if (backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;
            }
            else
            {
                PlayBackgroundMusic("BackgroundMusic");
            }

            audioSource.Play();
        }
        else
        {
            Destroy(gameObject); // Destroy the game object if it's not the first one
        }
    }

    void PlayBackgroundMusic(string clipName)
    {
        audioSource.clip = Resources.Load<AudioClip>(clipName);
        audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }
}
