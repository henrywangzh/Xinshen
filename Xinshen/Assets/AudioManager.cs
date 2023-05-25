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
    public Sound[] musicSounds, sfxSounds, ambientSounds;
    public AudioSource musicSource, sfxSource, ambientSource;

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


        ambientSource.volume = 0;
        ambientSource.loop = true;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        string nextName = next.name;

        if(nextName == "FirstLevel")
        {
            StartCoroutine(FadeIn(musicSource, 5f, 0.3f));
            playMusic("Region1BGMusic");
            PlayAmbient("AmbientMusic");
        }

    }

    public void PlayAmbient(string name)
    {
        Sound s = Array.Find(ambientSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Ambient sound not found");
        }
        else
        {
            ambientSource.clip = s.clip;
            ambientSource.volume = 0.1f;
            ambientSource.Play();
            StartCoroutine(RandomlyPlayAmbientSound(name));
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
        audioSource.volume = 0;
        currentlyFading = false;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float volume)
    {
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < volume)
        {
            audioSource.volume += volume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.volume = volume;
    }

    IEnumerator RandomlyPlayAmbientSound(string name)
    {
        float time = 0;
        float ambientSoundLength = 0;
        Sound s = Array.Find(ambientSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Ambient sound not found");
        }
        else
        {
            ambientSoundLength = s.clip.length;
        }
        while (true)
        {
            ambientSource.time = time;
            if(ambientSource.time >= ambientSoundLength)
            {
                ambientSource.time = 0;
            }
            StartCoroutine(FadeIn(ambientSource, 2f, 0.2f));
            float randomWaitTime = UnityEngine.Random.Range(5f, 10f);
            time += randomWaitTime;
            yield return new WaitForSeconds(randomWaitTime);
            StartCoroutine(FadeOut(ambientSource, 2f));
            randomWaitTime = UnityEngine.Random.Range(5f, 10f);
            yield return new WaitForSeconds(randomWaitTime);

        }

    }
}
