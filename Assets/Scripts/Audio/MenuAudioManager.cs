using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip menuMusic;

    public void PlayMenuMusic() {

        musicSource.clip = menuMusic;
        musicSource.Play();

    }
}
