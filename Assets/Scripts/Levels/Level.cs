using UnityEngine;

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
    [SerializeField] private Color defaultRopeStartColor;
    [SerializeField] private Color defaultRopeEndColor;

    [Header("Movement")]
    [SerializeField] private bool lookEnabled;
    [SerializeField] private bool walkEnabled;
    [SerializeField] private bool sprintEnabled;
    [SerializeField] private bool jumpEnabled;
    [SerializeField] private bool crouchEnabled;
    [SerializeField] private bool slideEnabled;
    [SerializeField] private bool wallRunEnabled;
    [SerializeField] private bool swingEnabled;
    [SerializeField] private bool ziplineEnabled;
    [SerializeField] private bool grabEnabled;

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

    public bool GetLookEnabled() {

        return lookEnabled;

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

    public bool GetGrabEnabled() {

        return grabEnabled;

    }
}