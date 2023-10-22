using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip everythingIsAwesomeMusic;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip grappleSound;
    [SerializeField] private AudioClip victorySound;

    public enum MusicType {

        EverythingIsAwesome

    }

    public enum SoundEffectType {

        Land, Grapple, Victory

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

            case SoundEffectType.Land:

            soundEffectSource.PlayOneShot(landSound);
            break;

            case SoundEffectType.Grapple:

            soundEffectSource.PlayOneShot(grappleSound);
            break;

            case SoundEffectType.Victory:

            soundEffectSource.PlayOneShot(victorySound);
            break;

            default:

            break;

        }
    }
}
