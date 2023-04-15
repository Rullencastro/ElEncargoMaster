using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource,sfxSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        PlayMusic("Theme");
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, s => s.name == name);

        if (sound != null)
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);

        if (sound != null)
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    private IEnumerator PlaySoundEvery(string name, float t, int times)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);

        for (int i = 0; i < times; i++)
        {
            PlaySFX(name);
            yield return new WaitForSeconds(t);
        }
    }

    public void ButtonClick()
    {
        PlaySFX("Click");
    }

    public void ClockSound(float t, int times)
    {
        StartCoroutine(PlaySoundEvery("Clock", t, times));
    }

    public void StopClockSound()
    {
        StopAllCoroutines();
    }
}
