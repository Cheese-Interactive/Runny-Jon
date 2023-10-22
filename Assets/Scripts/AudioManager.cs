using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip everythingIsAwesomeMusic;
    [SerializeField] private AudioClip clickSound;

    public enum MusicType {

        EverythingIsAwesome

    }

    public enum SoundEffectType {

        Click

    }

    private void Start() {

        musicSource.loop = true;

    }

    public void PlayMusic(MusicType musicType) {

        switch (musicType) {

            case MusicType.EverythingIsAwesome:

            musicSource.clip = everythingIsAwesomeMusic;
            musicSource.Play();
            break;

            default:

            break;

        }
    }

    public void PlaySound(SoundEffectType soundType) {

        switch (soundType) {

            case SoundEffectType.Click:

            soundEffectSource.PlayOneShot(clickSound);
            break;

            default:

            break;

        }
    }
}
