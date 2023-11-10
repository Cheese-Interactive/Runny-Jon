using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Level : ScriptableObject {

    [Header("Settings")]
    [SerializeField] private string levelName;
    [SerializeField] private int ID;
    [SerializeField] private Object scene;
    [SerializeField] private Sprite icon;
    [SerializeField][Range(1, 3599)] private int timeLimit;
    [SerializeField] private bool isTutorial;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Movement")]
    [SerializeField] private bool walkEnabled;
    [SerializeField] private bool sprintEnabled;
    [SerializeField] private bool jumpEnabled;
    [SerializeField] private bool crouchEnabled;
    [SerializeField] private bool slideEnabled;
    [SerializeField] private bool wallRunEnabled;
    [SerializeField] private bool swingEnabled;
    [SerializeField] private bool ziplineEnabled;

    public string GetLevelName() {

        return levelName;

    }

    public int GetID() {

        return ID;

    }

    public Object GetScene() {

        return scene;

    }

    public Sprite GetIcon() {

        return icon;

    }

    public int GetTimeLimit() {

        return timeLimit;

    }

    public bool GetIsTutorial() {

        return isTutorial;

    }

    public AudioClip GetBackgroundMusic() {

        return backgroundMusic;

    }

    public bool GetWalkEnabled() {

        return walkEnabled;

    }

    public bool GetSprintEnabled() {

        return sprintEnabled;

    }

    public bool GetJumpEnabled() {

        return jumpEnabled;

    }

    public bool GetCrouchEnabled() {

        return crouchEnabled;

    }

    public bool GetSlideEnabled() {

        return slideEnabled;

    }

    public bool GetWallRunEnabled() {

        return wallRunEnabled;

    }

    public bool GetSwingEnabled() {

        return swingEnabled;

    }

    public bool GetZiplineEnabled() {

        return ziplineEnabled;

    }
}