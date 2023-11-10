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
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip victorySound;

    public enum GameSoundEffectType {

        WalkFootstep, SprintFootstep, Land, Grapple, Checkpoint, Death, Victory

    }

    private void Awake() {

        gameManager = FindObjectOfType<GameManager>();
        musicSource.loop = true;

    }

    public void PlaySceneMusic() {

        musicSource.clip = gameManager.GetCurrentLevel().GetBackgroundMusic();
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

            case GameSoundEffectType.Checkpoint:

            soundEffectSource.PlayOneShot(checkpointSound);
            break;

            case GameSoundEffectType.Death:

            soundEffectSource.PlayOneShot(deathSound);
            break;

            case GameSoundEffectType.Victory:

            soundEffectSource.PlayOneShot(victorySound);
            break;

            default:

            break;

        }
    }
}
