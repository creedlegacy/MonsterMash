using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource, sfxSource, eternalSFXSource;
    [Header("Music Audio Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip mainPartyMusic;
    [Header("SoundFX Audio Clips")]
    public AudioClip partyBackgroundNoise;
    public AudioClip buttonSound;
    private float lastMusicTime;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            musicSource.clip = mainMenuMusic;
            musicSource.Play();
        }
        else if (SceneManager.GetActiveScene().name == "PartyLevelScene")
        {
            musicSource.clip = mainPartyMusic;
            musicSource.Play();
            eternalSFXSource.clip = partyBackgroundNoise;
            eternalSFXSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeRunningMusic(AudioClip clip)
    {
        if(musicSource.clip == mainPartyMusic)
        {
            lastMusicTime = musicSource.time;
            
        }

        musicSource.clip = clip;
        musicSource.time = 0.0f;
        musicSource.Play();

    }

    public void ReturnNormalMusic()
    {
        if (lastMusicTime != 0.0f)
        {
            musicSource.clip = mainPartyMusic;
            musicSource.time = lastMusicTime;
            musicSource.Play();
            lastMusicTime = 0.0f;
        }


    }
}
