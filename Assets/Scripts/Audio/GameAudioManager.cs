using UnityEngine;

public class GameAudioManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource soundEffectSource;
    private GameManager gameManager;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip walkFootstepSound;
    [SerializeField] private AudioClip sprintFootstepSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip grappleSound;
    [SerializeField] private AudioClip victorySound;

    public enum SoundEffectType {

        WalkFootstep, SprintFootstep, Land, Grapple, Victory

    }

    private void Awake() {

        gameManager = FindObjectOfType<GameManager>();
        musicSource.loop = true;

    }

    public void PlaySceneMusic() {

        musicSource.clip = gameManager.GetCurrentLevel().backgroundMusic;
        musicSource.Play();

    }

    public void PlaySound(SoundEffectType soundType) {

        switch (soundType) {

            case SoundEffectType.WalkFootstep:

            if (!footstepSource.isPlaying)
                footstepSource.PlayOneShot(walkFootstepSound);
            break;

            case SoundEffectType.SprintFootstep:

            if (!footstepSource.isPlaying)
                footstepSource.PlayOneShot(sprintFootstepSound);
            break;

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
