using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager audioManager;

    private void Awake()
    {
        if(audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public void playMusic(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogError("Music not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void playSound(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void playRepeatedSound(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound not found");
        }
        else
        {
            if (sfxSource.isPlaying) return;
            sfxSource.clip = s.clip;
            sfxSource.volume = 1;
            sfxSource.Play();
            sfxSource.loop = true;

        }
    }

    public void stopRepeatedSound()
    {
        if(!currentlyFading)
        {
            StartCoroutine(FadeOut(sfxSource, 0.1f));
        }
        sfxSource.loop = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        playMusic("MainMenuTheme");
        musicSource.loop = true;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        string nextName = next.name;

        if(nextName == "FirstLevel")
        {
            StartCoroutine(FadeIn(musicSource, 5f, 0.3f));
            playMusic("Region1BGMusic");
        }

    }

    bool currentlyFading = false;
    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        currentlyFading = true;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        currentlyFading = false;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float volume)
    {
        audioSource.volume = 0;
        while (audioSource.volume < volume)
        {
            audioSource.volume += volume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.volume = volume;
    }
}
