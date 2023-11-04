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

    public enum GameSoundEffectType {

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

    public void PlaySound(GameSoundEffectType soundType) {

        switch (soundType) {

            case GameSoundEffectType.WalkFootstep:

            if (!footstepSource.isPlaying)
                footstepSource.PlayOneShot(walkFootstepSound);
            break;

            case GameSoundEffectType.SprintFootstep:

            if (!footstepSource.isPlaying)
                footstepSource.PlayOneShot(sprintFootstepSound);
            break;

            case GameSoundEffectType.Land:

            soundEffectSource.PlayOneShot(landSound);
            break;

            case GameSoundEffectType.Grapple:

            soundEffectSource.PlayOneShot(grappleSound);
            break;

            case GameSoundEffectType.Victory:

            soundEffectSource.PlayOneShot(victorySound);
            break;

            default:

            break;

        }
    }
}
