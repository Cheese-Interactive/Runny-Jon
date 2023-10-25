using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;
    private GameManager gameManager;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip grappleSound;
    [SerializeField] private AudioClip victorySound;

    public enum SoundEffectType
    {

        Land, Grapple, Victory

    }

    private void Awake()
    {

        gameManager = FindObjectOfType<GameManager>();
        musicSource.loop = true;

    }

    public void PlayMenuMusic()
    {

        musicSource.clip = menuMusic;
        musicSource.Play();

    }

    public void PlaySceneMusic()
    {

        //musicSource.clip = gameManager.GetCurrentLevel().backgroundMusic;
        musicSource.Play();

    }

    public void PlaySound(SoundEffectType soundType)
    {

        switch (soundType)
        {

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
