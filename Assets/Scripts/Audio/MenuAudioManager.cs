using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameAudioManager;

public class MenuAudioManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;

    public enum UISoundEffectType {

        Click, Success, Error

    }

    private void Start() {

        musicSource.loop = true;

    }

    public void PlayMenuMusic() {

        musicSource.clip = menuMusic;
        musicSource.Play();

    }

    public void PlaySound(UISoundEffectType soundType) {

        switch (soundType) {

            case UISoundEffectType.Click:

            soundEffectSource.PlayOneShot(clickSound);
            break;

            case UISoundEffectType.Success:

            soundEffectSource.PlayOneShot(successSound);
            break;

            case UISoundEffectType.Error:

            soundEffectSource.PlayOneShot(errorSound);
            break;

            default:

            break;

        }
    }
}
